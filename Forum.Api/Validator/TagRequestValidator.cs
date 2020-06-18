using FluentValidation;
using Forum.Api.Requests;

namespace Forum.Api.Validator
{
    public class TagRequestValidator : AbstractValidator<TagRequest>
    {
        public TagRequestValidator()
        {
            RuleFor(tagRequest => tagRequest.Id).GreaterThanOrEqualTo(0).WithMessage("Invalid Tag Id");

            RuleFor(tagRequest => tagRequest.Name).NotEmpty().WithMessage("Tag Is Empty.");
            RuleFor(tagRequest => tagRequest.Name).MaximumLength(55).WithMessage("Tag Name Is Too Long.");
        }
    }
}