using Application.Abstractions.Messaging;
using System;

namespace Application.Tasks.Complete;

public sealed record CompleteTaskCommand(
    Guid TaskId,
    Guid UserId) : ICommand;
