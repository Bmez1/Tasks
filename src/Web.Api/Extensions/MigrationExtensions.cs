using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Web.Api.Extensions;

public static class MigrationExtensions
{
    public static void ApplyMigrations(this IApplicationBuilder app, ApplicationDbContext dbContext)
    {
        dbContext.Database.EnsureDeleted();
        dbContext.Database.Migrate();
        dbContext.Database.EnsureCreated();
    }
}
