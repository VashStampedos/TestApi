using TestApi.Domain.Services.Auth;
using TestApi.Domain.Services.UserServices;

namespace TestApi.Configures.Services
{
    public static class ConfigureServices
    {
        public static void AddUserServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAuthService, AuthService>();



        }
    }
}
