using FluentValidation;
using System;

namespace Application.Tasks.Update;

public sealed class UpdateTaskCommandValidator : AbstractValidator<UpdateTaskCommand>
{
    public UpdateTaskCommandValidator()
    {
        RuleFor(x => x.TaskId)
            .NotEmpty()
            .WithMessage("La tarea es requerida.");

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("La descripción es requerida.")
            .MaximumLength(500)
            .WithMessage("La descripción no puede superar los 500 carácteres.");

        RuleFor(x => x.CategoryId)
            .NotEmpty()
            .WithMessage("La categoría es requerida.");

        RuleFor(x => x.DueDate)
            .Must(date => !date.HasValue || date.Value > DateTime.UtcNow.AddDays(-1))
            .WithMessage("La fecha limite debe ser mayor a la fecha actual.");
    }
}
