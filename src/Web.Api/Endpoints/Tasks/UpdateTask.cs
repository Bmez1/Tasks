using Application.Abstractions.Authentication;
using Application.Abstractions.Messaging;
using Application.Tasks.Update;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;
using System;
using Microsoft.AspNetCore.Http;

namespace Web.Api.Endpoints.Tasks;

internal sealed class UpdateTask : IEndpoint
{
    /// <summary>
    /// Represents a request to update an existing task.
    /// </summary>
    public sealed class Request
    {
        /// <summary>
        /// The updated description of the task.
        /// </summary>
        public string Description { get; set; }
        
        /// <summary>
        /// The updated ID of the category for the task.
        /// </summary>
        public Guid CategoryId { get; set; }
        
        /// <summary>
        /// The updated optional due date for the task.
        /// </summary>
        public DateTime? DueDate { get; set; }

        /// <summary>
        /// Indicates whether the task is completed.
        /// </summary>
        public bool IsCompleted { get; set; }
    }

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("tasks/{id:guid}", async (
            Guid id,
            Request request,
            IUserContext userContext,
            ICommandHandler<UpdateTaskCommand> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new UpdateTaskCommand(
                id,
                userContext.UserId,
                request.Description,
                request.CategoryId,
                request.DueDate,
                request.IsCompleted);

            Result result = await handler.Handle(command, cancellationToken);

            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .WithTags(Tags.Tasks)
        .WithSummary("Updates an existing task.")
        .WithDescription("Allows an authenticated user to update an existing task, including its description, category, due date, and completion status.")
        .Accepts<Request>("application/json")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status400BadRequest, contentType: "application/json")
        .RequireAuthorization();
    }
}
