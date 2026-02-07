using System.Collections.Generic;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Categories.GetCategories;

internal sealed class GetCategoriesQueryHandler(IApplicationDbContext context)
    : IQueryHandler<GetCategoriesQuery, List<CategoryResponse>>
{
    public async Task<Result<List<CategoryResponse>>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
    {
        List<CategoryResponse> categories = await context
            .Categories
            .Select(category => new CategoryResponse(category.Id, category.Name))
            .ToListAsync(cancellationToken);

        return Result.Success(categories, categories.Count);
    }
}
