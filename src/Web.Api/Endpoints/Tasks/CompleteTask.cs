using Application.Abstractions.Authentication;
using Application.Abstractions.Messaging;
using Application.Tasks.Complete;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;
using System;
using Microsoft.AspNetCore.Http;

namespace Web.Api.Endpoints.Tasks;

internal sealed class CompleteTask : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("tasks/{id:guid}/complete", async (
            Guid id,
            IUserContext userContext,
            ICommandHandler<CompleteTaskCommand> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new CompleteTaskCommand(id, userContext.UserId);

            Result result = await handler.Handle(command, cancellationToken);

            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .WithTags(Tags.Tasks)
        .WithSummary("Marks a task as completed.")
        .WithDescription("Allows an authenticated user to mark one of their tasks as completed.")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status400BadRequest, contentType: "application/json")
        .RequireAuthorization();
    }
}
