using FluentValidation;
using Forum.Api.Requests;

namespace Forum.Api.Validator
{
    public class UserRequestValidator : AbstractValidator<AssignRoleRequest>
    {
        public UserRequestValidator()
        {
            RuleFor(r => r.UserName)
                .MinimumLength(3)
                .WithMessage("UserName is too short");
            RuleFor(r => r.UserName)
                .MaximumLength(55)
                .WithMessage("UserName is too long");
            
            
            RuleFor(r => r.RoleName)
                .MinimumLength(4)
                .WithMessage("RoleName is too short");
            RuleFor(r => r.RoleName)
                .MaximumLength(55)
                .WithMessage("RoleName is too long");
        }
    }
}