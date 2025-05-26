using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Products.EditProduct
{
    public class EditProductValidator : AbstractValidator<EditProductCommand>
    {
        public EditProductValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(p => p.Name).NotEmpty().Length(3, 50);
        }
    }
}
