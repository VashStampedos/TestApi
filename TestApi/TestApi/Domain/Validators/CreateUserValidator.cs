using FluentValidation;
using TestApi.DTO.User;

namespace TestApi.Validators
{
    public class CreateUserValidator:AbstractValidator<CreateUserRequest>
    {
        public CreateUserValidator()
        {
            RuleFor(x=> x.Age).NotEmpty().GreaterThan(0);
            RuleFor(x => x.Email).EmailAddress();
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.RoleId).NotEmpty();
        }
    }
}
