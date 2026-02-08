using Application.Abstractions.Authentication;
using Application.Abstractions.Messaging;
using Application.Tasks;
using Application.Tasks.GetFiltered;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace Web.Api.Endpoints.Tasks;

internal sealed class GetFilteredTasks : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("tasks", async (
            Guid? categoryId,
            IUserContext userContext,
            IQueryHandler<GetFilteredTasksQuery, List<TaskResponse>> handler,
            CancellationToken cancellationToken) =>
        {
            var query = new GetFilteredTasksQuery(
                userContext.UserId,
                categoryId);

            Result<List<TaskResponse>> result = await handler.Handle(query, cancellationToken);

            return result.ToHttpResponse();
        })
        .WithTags(Tags.Tasks)
        .WithSummary("Gets filtered tasks for the current user.")
        .WithDescription("Retrieves a list of tasks for the authenticated user, with optional filtering by category.")
        .Produces<List<TaskResponse>>(StatusCodes.Status200OK, "application/json")
        .RequireAuthorization();
    }
}
