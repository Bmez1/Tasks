using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Categories;
using Microsoft.EntityFrameworkCore;
using SharedKernel;
using System.Threading;
using System.Threading.Tasks;
using Task = Domain.Tasks.Task;

namespace Application.Tasks.Create;

public sealed class CreateTaskCommandHandler(IApplicationDbContext context)
    : ICommandHandler<CreateTaskCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
    {
        // Database-level validation
        bool categoryExists = await context.Categories
            .AnyAsync(c => c.Id == request.CategoryId, cancellationToken);

        if (!categoryExists)
        {
            return Result.Failure<Guid>(CategoryErrors.NotFound(request.CategoryId));
        }

        var task = Task.Create(
            request.UserId,
            request.Name,
            request.Description,
            request.CategoryId,
            request.DueDate);

        context.Tasks.Add(task);

        await context.SaveChangesAsync(cancellationToken);

        return task.Id;
    }
}
