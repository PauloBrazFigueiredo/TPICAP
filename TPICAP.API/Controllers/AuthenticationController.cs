using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using TPICAP.API.Interfaces;
using TPICAP.API.Models;

namespace TPICAP.API.Controllers
{
    [Route("api/authentication")]
    public class AuthenticationController : ApiControllerBase
    {
        private readonly ILogger<AuthenticationController> Logger;
        private readonly IAuthenticationService AuthenticationService;

        public AuthenticationController(ILogger<AuthenticationController> logger, IAuthenticationService authenticationService)
        {
            this.Logger = logger ??
                throw new ArgumentException(nameof(logger));
            this.AuthenticationService = authenticationService ??
                throw new ArgumentException(nameof(authenticationService)); ;
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(PersonResponseModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(PersonResponseModel), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Login(LoginModel login)
        {
            string token = await this.AuthenticationService.CreateJwtSecurityToken(login);
            if (string.IsNullOrEmpty(token))
            {
                return BadRequest(new { message = "Invalid login" });
            }
            return Ok(new { token });
        }
    }
}
