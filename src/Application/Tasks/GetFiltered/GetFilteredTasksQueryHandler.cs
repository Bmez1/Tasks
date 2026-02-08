using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using SharedKernel;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Task = Domain.Tasks.Task;

namespace Application.Tasks.GetFiltered;

internal sealed class GetFilteredTasksQueryHandler(
    IApplicationDbContext context,
    IUserContext userContext)
    : IQueryHandler<GetFilteredTasksQuery, List<TaskResponse>>
{
    public async Task<Result<List<TaskResponse>>> Handle(GetFilteredTasksQuery request, CancellationToken cancellationToken)
    {
        // Validate if the requesting user ID matches the query user ID
        if (request.UserId != userContext.UserId)
        {
            return Result.Failure<List<TaskResponse>>(UserErrors.Unauthorized());
        }

        IQueryable<Task> tasksQuery = context.Tasks
            .Where(task => task.UserId == request.UserId)
            .Include(t => t.Category);

        if (request.CategoryId.HasValue && request.CategoryId != Guid.Empty)
        {
            tasksQuery = tasksQuery.Where(task => task.CategoryId == request.CategoryId.Value);
        }

        List<TaskResponse> taskResponses = await tasksQuery
            .Select(task => new TaskResponse
            {
                Id = task.Id,
                Name = task.Name,
                Description = task.Description,
                CategoryId = task.CategoryId,
                CategoryName = task.Category.Name,
                DueDate = task.DueDate,
                IsCompleted = task.IsCompleted,
                CreatedAt = task.CreatedAt,
                CompletedAt = task.CompletedAt
            })
            .ToListAsync(cancellationToken);

        return Result.Success(taskResponses, taskResponses.Count);
    }
}
