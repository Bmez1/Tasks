using Domain.Categories;
using SharedKernel;
using System;
using System.Collections.Generic;

namespace Domain.Tasks;

public sealed class Task : Entity
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public string Description { get; private set; }
    public Guid CategoryId { get; private set; }
    public Category Category { get; set; }
    public DateTime? DueDate { get; private set; }
    public bool IsCompleted { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? CompletedAt { get; private set; }

    // Private constructor for EF Core and internal creation
    private Task() { }

    private Task(
        Guid id,
        Guid userId,
        string description,
        Guid categoryId,
        DateTime? dueDate)
    {
        Id = id;
        UserId = userId; // Set UserId
        Description = description;
        CategoryId = categoryId;
        DueDate = dueDate;
        IsCompleted = false;
        CreatedAt = DateTime.UtcNow;
    }

    public static Task Create(
        Guid userId,
        string description,
        Guid categoryId,
        DateTime? dueDate)
    {
        return new Task(Guid.NewGuid(), userId, description, categoryId, dueDate);
    }

    public void MarkComplete()
    {
        if (!IsCompleted)
        {
            IsCompleted = true;
            CompletedAt = DateTime.UtcNow;
        }
    }

    public void Update(string description, Guid categoryId, DateTime? dueDate, bool isCompleted)
    {
        Description = description;
        CategoryId = categoryId;
        DueDate = dueDate;
        IsCompleted = isCompleted;
        if (isCompleted && !CompletedAt.HasValue)
        {
            CompletedAt = DateTime.UtcNow;
        }
        else if (!isCompleted && CompletedAt.HasValue)
        {
            CompletedAt = null;
        }
    }
}
