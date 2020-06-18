using FluentValidation;
using Forum.Api.Requests;

namespace Forum.Api.Validator
{
    public class ReplyRequestValidator : AbstractValidator<ReplyRequest>
    {
        public ReplyRequestValidator()
        {
            RuleFor(r => r.Id).GreaterThanOrEqualTo(0).WithMessage("Invalid Reply Id");

            RuleFor(r => r.Text).MinimumLength(10).WithMessage("Reply is too short");
            RuleFor(r => r.Text).MaximumLength(1000).WithMessage("Reply is too long");

            RuleFor(r => r.PostId).NotEmpty().WithMessage("Reply should have post id");
        }
    }
}