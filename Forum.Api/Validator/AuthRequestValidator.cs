using FluentValidation;
using Forum.Api.Requests;

namespace Forum.Api.Validator
{
    public class AuthRequestValidator : AbstractValidator<AuthenticateRequest>
    {
        public AuthRequestValidator()
        {
            RuleFor(r => r.UserName).MaximumLength(55).WithMessage("Username is too long");
            RuleFor(r => r.UserName).MinimumLength(3).WithMessage("Username is too short");

            RuleFor(r => r.Password).NotEmpty();
        }
    }
}