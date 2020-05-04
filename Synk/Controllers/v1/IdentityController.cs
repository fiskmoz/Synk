using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Synk.Contracts;
using Synk.Contracts.v1;
using Synk.Contracts.v1.Requests;
using Synk.Contracts.v1.Responses;
using Synk.Domain;
using Synk.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Synk.Controllers.v1
{
    public class IdentityController : Controller
    {
        private readonly IIdentityService _identityService;
        public IdentityController(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        [EnableCors("localhost")]
        [HttpPost(ApiRoutes.Identity.Register, Name = EndpointNames.Identity.Register)]
        public async Task<ActionResult<AuthSuccessResponse>> Register([FromBody] UserRegistrationRequest request)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(new AuthFailedResponse
                {
                    Errors = ModelState.Values.SelectMany(x => x.Errors.Select(xx => xx.ErrorMessage))
                });
            }
            var authResponse = await _identityService.RegisterAsync(request.Email, request.Password);
            return ValidateAuthResponse(authResponse);
        }

        [EnableCors("localhost")]
        [HttpPost(ApiRoutes.Identity.Login, Name = EndpointNames.Identity.Login)]
        public async Task<ActionResult<AuthSuccessResponse>> Login([FromBody] UserLoginRequest request)
        {
            var authResponse = await _identityService.LoginAsync(request.Email, request.Password);
            return ValidateAuthResponse(authResponse);
        }

        [EnableCors("localhost")]
        [HttpPost(ApiRoutes.Identity.Refresh, Name = EndpointNames.Identity.Refresh)]
        public async Task<ActionResult<AuthSuccessResponse>> Login([FromBody] RefreshTokenRequest request)
        {
            var authResponse = await _identityService.RefreshTokenAsync(request.Token, request.RefreshToken);
            return ValidateAuthResponse(authResponse);
        }

        private ActionResult<AuthSuccessResponse> ValidateAuthResponse(AuthenticationResultDto authResponse)
        {
            if (!authResponse.Success)
            {
                return BadRequest(new AuthFailedResponse
                {
                    Errors = authResponse.ErrorMessages
                });
            };
            return Ok(new AuthSuccessResponse
            {
                Token = authResponse.Token,
                RefreshToken = authResponse.RefreshToken
            });
        }
    }
}
