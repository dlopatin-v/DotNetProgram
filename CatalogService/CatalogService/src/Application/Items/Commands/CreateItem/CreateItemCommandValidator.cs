using FluentValidation;

namespace CatalogService.Application.Items.Commands.CreateItem;

public class CreateItemCommandValidator : AbstractValidator<CreateItemCommand>
{
    public CreateItemCommandValidator()
    {
        RuleFor(v => v.Name)
            .MaximumLength(50)
            .NotEmpty().WithMessage("Name should be less then 50 symbols and not empty");
    }
}