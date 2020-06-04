using FluentValidation;
using Forum.Api.Requests;

namespace Forum.Api.Validator
{
    public class RoleRequestValidator : AbstractValidator<RoleRequest>
    {
        public RoleRequestValidator()
        {
            RuleFor(r => r.Name).MinimumLength(4).WithMessage("Min length for role name is 4 characters");
        }
    }
}