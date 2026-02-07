using Application.Abstractions.Messaging;
using Application.Tasks.Categories.Create;
using SharedKernel;
using Web.Api.Extensions;

namespace Web.Api.Endpoints.Categories;

internal sealed class CreateCategory : IEndpoint
{
    /// <summary>
    /// Represents a request to create a new category.
    /// </summary>
    public sealed class Request
    {
        /// <summary>
        /// The name of the category.
        /// </summary>
        public string Name { get; set; }
    }

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("categories", async (
            Request request,
            ICommandHandler<CreateCategoryCommand, Guid> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new CreateCategoryCommand(request.Name);

            Result<Guid> result = await handler.Handle(command, cancellationToken);

            return result.ToHttpResponse();
        })
        .WithTags(Tags.Categories)
        .WithSummary("Creates a new category.")
        .WithDescription("Allows users to create a new category for organizing tasks.")
        .Accepts<Request>("application/json")
        .Produces<Guid>(StatusCodes.Status201Created, "application/json")
        .RequireAuthorization();
    }
}
