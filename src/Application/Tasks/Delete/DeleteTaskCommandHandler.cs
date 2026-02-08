using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Categories;
using Domain.Tasks;
using Microsoft.EntityFrameworkCore;
using SharedKernel;
using Task = Domain.Tasks.Task;

namespace Application.Tasks.Delete;

internal sealed class DeleteTaskCommandHandler(IApplicationDbContext context)
    : ICommandHandler<DeleteTaskCommand>
{
    public async Task<Result> Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
    {
        Task? task = await context.Tasks
            .Where(t => t.Id == request.TaskId && t.UserId == request.UserId)
            .FirstOrDefaultAsync(cancellationToken);

        if (task is null)
        {
            return Result.Failure(TaskErrors.NotFound(request.TaskId));
        }

        context.Tasks.Remove(task);

        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
