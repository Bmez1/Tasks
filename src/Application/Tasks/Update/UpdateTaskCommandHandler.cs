using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Categories;
using Domain.Tasks;
using Microsoft.EntityFrameworkCore;
using SharedKernel;
using System.Threading;
using System.Threading.Tasks;
using Task = Domain.Tasks.Task;

namespace Application.Tasks.Update;

internal sealed class UpdateTaskCommandHandler(IApplicationDbContext context)
    : ICommandHandler<UpdateTaskCommand>
{
    public async Task<Result> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
    {
        Task? task = await context.Tasks
            .Where(t => t.Id == request.TaskId && t.UserId == request.UserId)
            .FirstOrDefaultAsync(cancellationToken);

        if (task is null)
        {
            return Result.Failure(TaskErrors.NotFound(request.TaskId));
        }

        // Database-level validation for CategoryId
        bool categoryExists = await context.Categories
            .AnyAsync(c => c.Id == request.CategoryId, cancellationToken);

        if (!categoryExists)
        {
            return Result.Failure(CategoryErrors.NotFound(request.CategoryId));
        }

        task.Update(
            request.Name,
            request.Description,
            request.CategoryId,
            request.DueDate,
            request.IsCompleted);

        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
