using FluentValidation;
using System;

namespace Application.Tasks.Create;

public sealed class CreateTaskCommandValidator : AbstractValidator<CreateTaskCommand>
{
    public CreateTaskCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("El nombre es requerido.")
            .MaximumLength(80)
            .WithMessage("El nombre no puede superar los 80 carácteres.");

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("La descripción es requerida.")
            .MaximumLength(500)
            .WithMessage("La descripción no puede superar los 500 carácter.");

        RuleFor(x => x.CategoryId)
            .NotEmpty()
            .WithMessage("La categoría es requerida.");

        RuleFor(x => x.DueDate)
            .Must(date => !date.HasValue || date.Value > DateTime.UtcNow.AddDays(-1))
            .WithMessage("La fecha limite no debe ser menor a la actual.");
    }
}
