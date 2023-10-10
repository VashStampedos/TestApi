using FluentValidation;
using System.Reflection;
using System.Runtime.CompilerServices;
using TestApi.Domain.Validators;
using TestApi.Validators;

namespace TestApi.Configures.Validators
{
    public static class ConfigureValidators
    {

        public static void AddUserValidators(this IServiceCollection services)
        {
            services.AddScoped<CreateUserValidator>();
            services.AddScoped<UpdateUserValidator>();
            services.AddScoped<AddRoleToUserValidator>();
            services.AddScoped<UserListValidator>();
            services.AddScoped<LoginUserValidator>();
        }
    }
}
