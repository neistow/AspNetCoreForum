using FluentValidation;
using Forum.Api.Requests;

namespace Forum.Api.Validator
{
    public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
    {
        public RegisterRequestValidator()
        {
            RuleFor(r => r.Email)
                .EmailAddress()
                .WithMessage("Invalid Email Address");

            RuleFor(r => r.UserName)
                .MinimumLength(3)
                .WithMessage("UserName is too short");

            RuleFor(r => r.UserName)
                .MaximumLength(55)
                .WithMessage("UserName is too long");

            RuleFor(r => r.PasswordConfirm)
                .Equal(r => r.Password)
                .WithMessage("Passwords don't match");
        }
    }
}