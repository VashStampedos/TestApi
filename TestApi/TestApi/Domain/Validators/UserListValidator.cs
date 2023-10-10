using FluentValidation;
using TestApi.DTO.User;

namespace TestApi.Validators
{
    public class UserListValidator:AbstractValidator<UserListRequest>
    {
        public UserListValidator()
        {
            RuleFor(x => x.Offset).GreaterThanOrEqualTo(0);
            RuleFor(x => x.Count).NotEmpty();
            
        }
    }
}
