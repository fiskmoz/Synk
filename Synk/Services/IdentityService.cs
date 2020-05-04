using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Synk.Data;
using Synk.Domain;
using Synk.Installers;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Synk.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly JwtSettings _jwtSettings;
        private readonly TokenValidationParameters _tokenValidationParameters;
        private readonly ApplicationDbContext _dataContext;
        public IdentityService(UserManager<IdentityUser> userManager, JwtSettings jwtSettings, TokenValidationParameters tokenValidationParameters, ApplicationDbContext dataContext)
        {
            _jwtSettings = jwtSettings;
            _userManager = userManager;
            _tokenValidationParameters = tokenValidationParameters;
            _dataContext = dataContext;
        }

        public async Task<AuthenticationResultDto> LoginAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return new AuthenticationResultDto
                {
                    ErrorMessages = new[] { "User does not exist" }
                };
            }
            var userHasValidPassword = await _userManager.CheckPasswordAsync(user, password);
            if (!userHasValidPassword)
            {
                return new AuthenticationResultDto
                {
                    ErrorMessages = new[] { "User/password combination is wrong." }
                };
            }
            return await GenerateAuthenticationResultForUserAsync(user);
        }

        public async Task<AuthenticationResultDto> RefreshTokenAsync(string token, string refreshToken)
        {
            var validatedToken = GetPrincipalFromToken(token);
            if (validatedToken == null)
            {
                return new AuthenticationResultDto { ErrorMessages = new[] { "Invalid Token" } };
            }
            var expiryDateUnix = long.Parse(validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Exp).Value);
            var expiryDateTimeUtc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).
                AddSeconds(expiryDateUnix)
                .Add(_jwtSettings.TokenLifeTime);
            if (expiryDateTimeUtc > DateTime.UtcNow)
            {
                return new AuthenticationResultDto { ErrorMessages = new[] { "This token has not expired yet" } };
            }

            var jti = validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Jti).Value;
            var storedRefreshToken = await _dataContext.RefreshTokens.SingleOrDefaultAsync(x => x.Token == refreshToken);

            if(storedRefreshToken == null)
            {
                return new AuthenticationResultDto { ErrorMessages = new[] { "This tokes does not exist" } };
            }
            if(storedRefreshToken.Invalidated)
            {
                return new AuthenticationResultDto { ErrorMessages = new[] { "This token is invalid" } };
            }
            if (storedRefreshToken.Used)
            {
                return new AuthenticationResultDto { ErrorMessages = new[] { "This token has been used" } };
            }
            if (storedRefreshToken.JwtId != jti)
            {
                return new AuthenticationResultDto { ErrorMessages = new[] { "This token does not match" } };
            }

            storedRefreshToken.Used = true;
            _dataContext.Update(storedRefreshToken);
            await _dataContext.SaveChangesAsync();

            var user = await _userManager.FindByIdAsync(validatedToken.Claims.Single(x => x.Type == "id").Value);
            return await GenerateAuthenticationResultForUserAsync(user);
        }

        private ClaimsPrincipal GetPrincipalFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                var principal = tokenHandler.ValidateToken(token, _tokenValidationParameters, out var validatedToken);
                if(!IsJwtWithValidSecurityAlgorithm(validatedToken))
                {
                    return null;
                }
                return principal;
            }
            catch
            {
                return null;
            }
        }

        private bool IsJwtWithValidSecurityAlgorithm(SecurityToken validatedToken)
        {
            return (validatedToken is JwtSecurityToken jwtSecurityToken) &&
                jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, 
                StringComparison.InvariantCultureIgnoreCase);
        }

        public async Task<AuthenticationResultDto> RegisterAsync(string email, string password)
        {
            var existingUser = await _userManager.FindByEmailAsync(email);
            if (existingUser != null)
            {
                return new AuthenticationResultDto
                {
                    ErrorMessages = new[] { "User with this email already exists" }
                };
            }
            var newUser = new IdentityUser
            {
                Email = email,
                UserName = email
            };
            var createdUser = await _userManager.CreateAsync(newUser, password);

            if (!createdUser.Succeeded)
            {
                return new AuthenticationResultDto
                {
                    ErrorMessages = createdUser.Errors.Select(x => x.Description)
                };
            }

            return await GenerateAuthenticationResultForUserAsync(newUser);
        }

        private async Task<AuthenticationResultDto> GenerateAuthenticationResultForUserAsync(IdentityUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim("id", user.Id)
                }),
                Expires = DateTime.UtcNow.Add(_jwtSettings.TokenLifeTime),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            var refreshToken = new RefreshToken
            {
                JwtId = token.Id,
                UserId = user.Id,
                CreationDate = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddMonths(6)
            };
            await _dataContext.RefreshTokens.AddAsync(refreshToken);
            await _dataContext.SaveChangesAsync();

            return new AuthenticationResultDto
            {
                Success = true,
                RefreshToken = refreshToken.Token,
                Token = tokenHandler.WriteToken(token)
            };
        }
    }
}
