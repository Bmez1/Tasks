using FluentValidation;

namespace Application.Tasks.Categories.Create;

public sealed class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Category name cannot be empty.")
            .MaximumLength(100)
            .WithMessage("Category name cannot exceed 100 characters.");
    }
}
