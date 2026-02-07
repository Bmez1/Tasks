using SharedKernel;
using System;

namespace Domain.Categories;

public static class CategoryErrors
{
    public static Error NotFound(Guid categoryId) => Error.NotFound(
        "Categories.NotFound",
        $"La categoría con el Id = '{categoryId}' no fue encontrada.");

    public static readonly Error NameNotUnique = Error.Conflict(
        "Categories.NameNotUnique",
        "El nombre de la categoría proporcionado no es único.");
}
