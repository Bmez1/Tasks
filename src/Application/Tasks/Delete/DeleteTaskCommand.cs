using Application.Abstractions.Messaging;

namespace Application.Tasks.Delete;

public sealed record DeleteTaskCommand(
    Guid TaskId,
    Guid UserId) : ICommand;
