using FluentValidation;
using System;

namespace Application.Tasks.Update;

public sealed class UpdateTaskCommandValidator : AbstractValidator<UpdateTaskCommand>
{
    public UpdateTaskCommandValidator()
    {
        RuleFor(x => x.TaskId)
            .NotEmpty()
            .WithMessage("Task ID cannot be empty.");

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Description cannot be empty.")
            .MaximumLength(500)
            .WithMessage("Description cannot exceed 500 characters.");

        RuleFor(x => x.CategoryId)
            .NotEmpty()
            .WithMessage("Category is required.");

        RuleFor(x => x.DueDate)
            .Must(date => !date.HasValue || date.Value > DateTime.UtcNow.AddDays(-1))
            .WithMessage("Due date cannot be in the past.");
    }
}
