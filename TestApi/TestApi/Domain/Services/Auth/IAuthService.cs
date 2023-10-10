using TestApi.DB.Entities;
using TestApi.DTO.User;

namespace TestApi.Domain.Services.Auth
{
    public interface IAuthService
    {
        public Task<User> AuthenticateAsync(LoginRequest userLogin);
        public string GenerateToken(User user);
    }
}
