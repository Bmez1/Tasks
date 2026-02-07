using Application.Abstractions.Messaging;
using Application.Categories.GetCategories;
using SharedKernel;
using Web.Api.Extensions;

namespace Web.Api.Endpoints.Categories;

public sealed class GetCategories : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("categories", async (
        IQueryHandler<GetCategoriesQuery, List<CategoryResponse>> handler,
        CancellationToken cancellationToken) =>
        {
            Result<List<CategoryResponse>> result = await handler.Handle(new GetCategoriesQuery(), cancellationToken);
            return result.ToHttpResponse();
        })
        .WithTags(Tags.Categories)
        .WithSummary("Get categories.")
        .WithDescription("Allows users get categories.")
        .RequireAuthorization();
    }
}
