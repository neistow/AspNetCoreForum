using FluentValidation;
using Forum.Api.Requests;

namespace Forum.Api.Validator
{
    public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
    {
        public RegisterRequestValidator()
        {
            RuleFor(r => r.Username).MaximumLength(55).WithMessage("UserName is too long");
            RuleFor(r => r.Username).MinimumLength(3).WithMessage("UserName is too short");

            RuleFor(r => r.Email).EmailAddress().WithMessage("Invalid Email");

            RuleFor(r => r.PasswordConfirm).Equal(r => r.Password)
                .WithMessage("Password Confirmation doesn't match password");
        }
    }
}