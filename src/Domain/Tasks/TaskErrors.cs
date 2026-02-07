using SharedKernel;
using System;

namespace Domain.Tasks;

public static class TaskErrors
{
    public static Error NotFound(Guid taskId) => Error.NotFound(
        "Tasks.NotFound",
        $"La tarea con el Id = '{taskId}' no fue encontrada.");

    public static Error Unauthorized() => Error.Failure(
        "Tasks.Unauthorized",
        "No estás autorizado para realizar esta acción en esta tarea.");

    public static readonly Error AlreadyCompleted = Error.Failure(
        "Tasks.AlreadyCompleted",
        "La tarea ya está completada.");
}
