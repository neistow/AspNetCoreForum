using FluentValidation;
using Forum.Api.Requests;

namespace Forum.Api.Validator
{
    public class PostRequestValidator : AbstractValidator<PostRequest>
    {
        public PostRequestValidator()
        {
            RuleFor(post => post.Id).GreaterThanOrEqualTo(1);

            RuleFor(post => post.Title).MaximumLength(55).WithMessage("Post Title Is Too Long.");
            RuleFor(post => post.Title).MinimumLength(10).WithMessage("Post Title Is Too Short.");

            RuleFor(post => post.Text).MinimumLength(100).WithMessage("Post Body Is Too Short.");
            RuleFor(post => post.Text).MaximumLength(5000).WithMessage("Post Body Is Too Long.");
        }
    }
}