using Domain.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Task = Domain.Tasks.Task;

namespace Infrastructure.Tasks;

internal sealed class TaskConfiguration : IEntityTypeConfiguration<Task>
{
    public void Configure(EntityTypeBuilder<Task> builder)
    {
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Description)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(t => t.CategoryId)
            .IsRequired();

        builder.Property(t => t.IsCompleted)
            .HasDefaultValue(false);

        builder.Property(t => t.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        // Configure the relationship with Category
        builder.HasOne(t => t.Category)
            .WithMany()
            .HasForeignKey(t => t.CategoryId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(t => t.UserId);
    }
}
