using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using TestApi.DB.Context;
using TestApi.DB.Entities;
using TestApi.Domain.Services.Auth;
using TestApi.Domain.Validators;
using TestApi.DTO.User;
using TestApi.Results;

namespace TestApi.Controllers
{
    [Route("api/v1/auth")]
    [ApiController]
    public class AuthController : Controller
    {
        IAuthService authService;
        LoginUserValidator loginValidator;
        public AuthController(IAuthService authService, LoginUserValidator loginValidator)
        {
            this.authService = authService;
            this.loginValidator = loginValidator;
        }
        [Route("login")]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var valid = await loginValidator.ValidateAsync(request);
            if (valid.IsValid)
            {
                var user =await authService.AuthenticateAsync(request);
            
                var token = authService.GenerateToken(user);
                return Ok(token);

            }
            return BadRequest(ApiResult<string>.Failure(HttpStatusCode.BadRequest, new List<string>() { "Invalid request" }));

            
        }

       
    }
}
