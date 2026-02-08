using Application.Abstractions.Messaging;
using System;
using System.Collections.Generic;

namespace Application.Tasks.Create;

public sealed record CreateTaskCommand(
    Guid UserId,
    string Name,
    string Description,
    Guid CategoryId,
    DateTime? DueDate) : ICommand<Guid>;

