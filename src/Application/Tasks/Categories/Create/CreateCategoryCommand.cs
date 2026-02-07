using Application.Abstractions.Messaging;
using System;

namespace Application.Tasks.Categories.Create;

public sealed record CreateCategoryCommand(
    string Name) : ICommand<Guid>;
