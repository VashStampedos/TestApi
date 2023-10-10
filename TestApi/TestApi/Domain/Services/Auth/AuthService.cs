using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TestApi.DB.Context;
using TestApi.DB.Entities;
using TestApi.Domain.Exceptions;
using TestApi.Domain.Services.Application;
using TestApi.Domain.Services.UserServices;
using TestApi.DTO.User;

namespace TestApi.Domain.Services.Auth
{
    public class AuthService: ApplicationContextService, IAuthService
    {
        
        IConfiguration config;
        IUserService userService;
        public AuthService(ApplicationContext context, IUserService userService, IConfiguration config):base(context)
        {
            this.config = config;
            this.userService = userService;
        }

        public async Task<User> AuthenticateAsync(LoginRequest userLogin)
        {
            var currentUser =await GetUserByEmailAsync(userLogin.Email);
            return currentUser;
           
        }
        public string GenerateToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,user.Email)
            };
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);


            return new JwtSecurityTokenHandler().WriteToken(token);

        }

        private async Task<User> GetUserByEmailAsync(string email)
        {
            var user =await context.Users.FirstOrDefaultAsync(x => x.Email.ToLower() ==
                email.ToLower());
            _ = user ?? throw new NotFoundException($"User with email {email} not found");
            return user;
        }

    }
}
