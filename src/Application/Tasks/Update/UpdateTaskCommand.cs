using Application.Abstractions.Messaging;
using System;
using System.Collections.Generic;

namespace Application.Tasks.Update;

public sealed record UpdateTaskCommand(
    Guid TaskId,
    Guid UserId,
    string Name,
    string Description,
    Guid CategoryId,
    DateTime? DueDate,
    bool IsCompleted) : ICommand;
