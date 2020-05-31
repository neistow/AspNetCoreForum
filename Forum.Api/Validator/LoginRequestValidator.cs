using FluentValidation;
using Forum.Api.Requests;

namespace Forum.Api.Validator
{
    public class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator()
        {
            RuleFor(r => r.UserName)
                .MinimumLength(3)
                .WithMessage("UserName is too short");
            
            RuleFor(r => r.UserName)
                .MaximumLength(55)
                .WithMessage("UserName is too long");

            RuleFor(r => r.Password)
                .NotEmpty()
                .WithMessage("Password is empty");
        }
    }
}