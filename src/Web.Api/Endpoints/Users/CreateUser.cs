using Application.Abstractions.Messaging;
using Application.Users.Register;
using SharedKernel;
using Web.Api.Extensions;

namespace Web.Api.Endpoints.Users;

internal sealed class CreateUser : IEndpoint
{
    public sealed record Request(string Email, string FirstName, string LastName, string Password);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("users/", async (
            Request request,
            ICommandHandler<RegisterUserCommand, Guid> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new RegisterUserCommand(request.Email, request.FirstName, request.LastName, request.Password);

            Result<Guid> result = await handler.Handle(command, cancellationToken);

            return result.ToHttpResponse();
        })
        .WithSummary("Registers a new user.")
        .WithDescription("Allows users register in the system. Email, FirstName, LastName and Password are required.")
        .WithTags(Tags.Users);
    }
}
