using Application.Abstractions.Messaging;
using System;
using System.Collections.Generic;

namespace Application.Tasks.GetFiltered;

public sealed record GetFilteredTasksQuery(
    Guid UserId,
    Guid? CategoryId) : IQuery<List<TaskResponse>>;
