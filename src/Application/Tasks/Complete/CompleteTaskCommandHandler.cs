using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Tasks;
using Microsoft.EntityFrameworkCore;
using SharedKernel;
using System.Threading;
using System.Threading.Tasks;
using Task = Domain.Tasks.Task;

namespace Application.Tasks.Complete;

public sealed class CompleteTaskCommandHandler(IApplicationDbContext context)
    : ICommandHandler<CompleteTaskCommand>
{
    public async Task<Result> Handle(CompleteTaskCommand request, CancellationToken cancellationToken)
    {
        Task? task = await context.Tasks
            .Where(t => t.Id == request.TaskId && t.UserId == request.UserId)
            .FirstOrDefaultAsync(cancellationToken);

        if (task is null)
        {
            return Result.Failure(TaskErrors.NotFound(request.TaskId));
        }

        if (task.IsCompleted)
        {
            return Result.Failure(TaskErrors.AlreadyCompleted);
        }

        task.MarkComplete();

        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
