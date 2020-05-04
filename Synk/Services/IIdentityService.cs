using Synk.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Synk.Services
{
    public interface IIdentityService
    {
        Task<AuthenticationResultDto> RegisterAsync(string email, string password);

        Task<AuthenticationResultDto> LoginAsync(string email, string password);
        Task<AuthenticationResultDto> RefreshTokenAsync(string email, string password);
    }
}
