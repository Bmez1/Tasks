using Application.Abstractions.Messaging;
using Application.Tasks.Create;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Application.Abstractions.Authentication; // Added for Results.Created

namespace Web.Api.Endpoints.Tasks;

internal sealed class CreateTask : IEndpoint
{
    /// <summary>
    /// Represents a request to create a new task.
    /// </summary>
    public sealed class Request
    {
        /// <summary>
        /// The name of the task.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The description of the task.
        /// </summary>
        public string Description { get; set; }
        
        /// <summary>
        /// The ID of the category for the task.
        /// </summary>
        public Guid CategoryId { get; set; }
        
        /// <summary>
        /// The optional due date for the task.
        /// </summary>
        public DateTime? DueDate { get; set; }
    }

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("tasks", async (
            Request request,
            ICommandHandler<CreateTaskCommand, Guid> handler,
            IUserContext userContext,
            CancellationToken cancellationToken) =>
        {
            var command = new CreateTaskCommand(
                userContext.UserId,
                request.Name,
                request.Description,
                request.CategoryId,
                request.DueDate);

            Result<Guid> result = await handler.Handle(command, cancellationToken);

            return result.ToHttpResponse();
        })
        .WithTags(Tags.Tasks)
        .WithSummary("Creates a new task.")
        .WithDescription("Allows users to create a new task with a description, category, and an optional due date.")
        .Accepts<Request>("application/json")
        .Produces<Guid>(StatusCodes.Status201Created, "application/json")
        .RequireAuthorization();
    }
}
