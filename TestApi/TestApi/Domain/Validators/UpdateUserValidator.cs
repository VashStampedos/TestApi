using FluentValidation;
using TestApi.DTO.User;

namespace TestApi.Validators
{
    public class UpdateUserValidator:AbstractValidator<UpdateUserRequest>
    {
        public UpdateUserValidator()
        {
            RuleFor(x => x.Age).NotEmpty().GreaterThan(0);
            RuleFor(x => x.Email).EmailAddress();
            RuleFor(x => x.Name).NotEmpty();
        }
    }
}
