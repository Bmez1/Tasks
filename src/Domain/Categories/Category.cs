using SharedKernel;
using System;

namespace Domain.Categories;

public sealed class Category : Entity
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }

    // Private constructor for EF Core and internal creation
    private Category() { }

    private Category(Guid id, string name)
    {
        Id = id;
        Name = name;
    }

    public static Category Create(string name, Guid? id = null)
    {
        return new Category(id ?? Guid.NewGuid(), name);
    }
}
