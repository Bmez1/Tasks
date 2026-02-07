using System;

namespace Application.Tasks;

public sealed class TaskResponse
{
    public Guid Id { get; init; }
    public string Description { get; init; }
    public Guid CategoryId { get; init; }
    public string CategoryName { get; init; } // To display the category name
    public DateTime? DueDate { get; init; }
    public bool IsCompleted { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime? CompletedAt { get; init; }
}
