using FluentValidation;
using Forum.Api.Requests;

namespace Forum.Api.Validator
{
    public class ChangePasswordRequestValidator : AbstractValidator<ChangePasswordRequest>
    {
        public ChangePasswordRequestValidator()
        {
            RuleFor(r => r.NewPassword)
                .Equal(r => r.NewPasswordConfirm)
                .WithMessage("Passwords don't match");
        }
    }
}