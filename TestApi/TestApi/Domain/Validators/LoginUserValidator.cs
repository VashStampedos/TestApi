using FluentValidation;
using TestApi.DTO.User;

namespace TestApi.Domain.Validators
{
    public class LoginUserValidator:AbstractValidator<LoginRequest>
    {
        public LoginUserValidator()
        {
            RuleFor(x => x.Email).EmailAddress();
        }
    }
}
