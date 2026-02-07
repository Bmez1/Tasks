using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Categories;
using SharedKernel;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Categories.Create;

public sealed class CreateCategoryCommandHandler(IApplicationDbContext context)
    : ICommandHandler<CreateCategoryCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = Category.Create(request.Name);

        context.Categories.Add(category);

        await context.SaveChangesAsync(cancellationToken);

        return category.Id;
    }
}
