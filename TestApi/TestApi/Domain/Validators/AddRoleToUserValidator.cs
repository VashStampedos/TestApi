using FluentValidation;
using TestApi.DTO.User;

namespace TestApi.Validators
{
    public class AddRoleToUserValidator:AbstractValidator<AddRoleToUserRequest>
    {
        public AddRoleToUserValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.RoleId).NotEmpty();
        }
    }
}
