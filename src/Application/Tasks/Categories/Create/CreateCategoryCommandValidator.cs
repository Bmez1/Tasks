using FluentValidation;

namespace Application.Tasks.Categories.Create;

public sealed class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("El nombre de la categoría es requerido.")
            .MaximumLength(100)
            .WithMessage("El nombre no puede superar los 100 carácteres.");
    }
}
