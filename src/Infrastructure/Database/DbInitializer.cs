using Application.Abstractions.Authentication;
using Domain.Categories;
using Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database;

public static class DbInitializer
{
    public static async Task SeedAsync(ApplicationDbContext context, IPasswordHasher passwordHasher)
    {
        // Seed default user
        bool changesExists = false;
        if (!await context.Users.AnyAsync())
        {
            var defaultUser = User.Create(
                "admin@example.com",
                "Admin",
                "User",
                passwordHasher.Hash("AdminP4ssw0rd!")
            );

            context.Users.Add(defaultUser);
            changesExists = true;
        }

        if (!await context.Categories.AnyAsync())
        {
            List<Category> categories =
                [
                Category.Create("Trabajo", Guid.Parse("a1b2c3d4-e5f6-7890-1234-567890abcdef")),
                Category.Create("Personal", Guid.Parse("b1c2d3e4-f5a6-7890-1234-567890abcdef")),
                Category.Create("Compras", Guid.Parse("c1d2e3f4-a5b6-7890-1234-567890abcdef")),
                Category.Create("Estudio", Guid.Parse("d1e2f3a4-b5c6-7890-1234-567890abcdef")),
                Category.Create("Salud", Guid.Parse("e1f2a3b4-c5d6-7890-1234-567890abcdef")),
                Category.Create("Ejercicio", Guid.Parse("f1a2b3c4-d5e6-7890-1234-567890abcdef")),
                Category.Create("Social", Guid.Parse("a2b3c4d5-e6f7-8901-2345-67890abcdef0")),
                Category.Create("Finanzas", Guid.Parse("b2c3d4e5-f6a7-8901-2345-67890abcdef1")),
                Category.Create("Hogar", Guid.Parse("c2d3e4f5-a6b7-8901-2345-67890abcdef2")),
                Category.Create("Creatividad", Guid.Parse("d2e3f4a5-b6c7-8901-2345-67890abcdef3"))
                ];

            context.Categories.AddRange(categories);
            changesExists = true;
        }

        if (changesExists)
        {
            await context.SaveChangesAsync();
        }
    }
}
