using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Product.EditProduct
{
    public class EditProductRequestValidator : AbstractValidator<EditProductRequest>
    {
        public EditProductRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(user => user.Name).NotEmpty().Length(3, 50);
        }
    }
}
