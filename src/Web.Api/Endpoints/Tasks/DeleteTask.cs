using Application.Abstractions.Authentication;
using Application.Abstractions.Messaging;
using Application.Tasks.Delete;
using Application.Tasks.Update;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Tasks;

internal sealed class DeleteTask : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("tasks/{id:guid}", async (
            Guid id,
            IUserContext userContext,
            ICommandHandler<DeleteTaskCommand> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new DeleteTaskCommand(
                id,
                userContext.UserId);

            Result result = await handler.Handle(command, cancellationToken);

            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .WithTags(Tags.Tasks)
        .WithSummary("Deletes an existing task.")
        .WithDescription("Allows an authenticated user to delete an existing task.")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status400BadRequest, contentType: "application/json")
        .RequireAuthorization();
    }
}
