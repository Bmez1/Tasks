using FluentValidation;
using System;

namespace Application.Tasks.Complete;

public sealed class CompleteTaskCommandValidator : AbstractValidator<CompleteTaskCommand>
{
    public CompleteTaskCommandValidator()
    {
        RuleFor(x => x.TaskId)
            .NotEmpty()
            .WithMessage("Task ID cannot be empty.");
    }
}
