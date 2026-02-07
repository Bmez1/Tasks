using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Users.Register;

using Domain.Users;

using Moq;
using Moq.EntityFrameworkCore;

using SharedKernel;

namespace UnitTests.Application.Users.Register;

public class RegisterUserCommandHandlerTests
{
    private readonly Mock<IApplicationDbContext> _contextMock;
    private readonly Mock<IPasswordHasher> _passwordHasherMock;
    private readonly RegisterUserCommandHandler _handler;

    public RegisterUserCommandHandlerTests()
    {
        _contextMock = new Mock<IApplicationDbContext>();
        _passwordHasherMock = new Mock<IPasswordHasher>();
        _handler = new RegisterUserCommandHandler(_contextMock.Object, _passwordHasherMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenEmailIsNotUnique()
    {
        // Arrange
        var command = new RegisterUserCommand("test@test.com", "John", "Doe", "Password123");
        var existingUser = User.Create(command.Email, "Existing", "User", "hashed");
        var users = new List<User> { existingUser };

        _contextMock.SetupGet(x => x.Users).ReturnsDbSet(users);

        // Act
        Result<Guid> result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(UserErrors.EmailNotUnique, result.Error);
        _contextMock.Verify(x => x.Users.Add(It.IsAny<User>()), Times.Never);
        _contextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenUserIsRegisteredSuccessfully()
    {
        // Arrange
        var command = new RegisterUserCommand("newuser@test.com", "Jane", "Smith", "NewPassword456");
        var users = new List<User>();

        _contextMock.SetupGet(x => x.Users).ReturnsDbSet(users);
        _passwordHasherMock.Setup(x => x.Hash(command.Password)).Returns("hashedpassword");

        // Act
        Result<Guid> result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotEqual(Guid.Empty, result.Value);
        _contextMock.Verify(x => x.Users.Add(It.Is<User>(u =>
            u.Email == command.Email &&
            u.FirstName == command.FirstName &&
            u.LastName == command.LastName &&
            u.PasswordHash == "hashedpassword")), Times.Once);
        _contextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
