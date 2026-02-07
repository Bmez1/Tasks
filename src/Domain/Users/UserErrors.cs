using SharedKernel;

namespace Domain.Users;

public static class UserErrors
{
    public static Error NotFound(Guid userId) => Error.NotFound(
        "Users.NotFound",
        $"El usuario con el Id = '{userId}' no fue encontrado");

    public static Error Unauthorized() => Error.Failure(
        "Users.Unauthorized",
        "No estás autorizado para realizar esta acción.");

    public static readonly Error NotFoundByEmail = Error.NotFound(
        "Users.NotFoundByEmail",
        "El usuario con el correo electrónico especificado no fue encontrado");

    public static readonly Error EmailNotUnique = Error.Conflict(
        "Users.EmailNotUnique",
        "El correo electrónico proporcionado ya está en uso");
}
