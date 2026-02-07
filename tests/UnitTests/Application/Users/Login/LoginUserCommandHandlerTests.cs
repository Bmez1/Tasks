using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Users.Login;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.EntityFrameworkCore;
using SharedKernel;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests.Application.Users.Login;

public class LoginUserCommandHandlerTests
{
    private readonly Mock<IApplicationDbContext> _contextMock;
    private readonly Mock<IPasswordHasher> _passwordHasherMock;
    private readonly Mock<ITokenProvider> _tokenProviderMock;
    private readonly LoginUserCommandHandler _handler;

    public LoginUserCommandHandlerTests()
    {
        _contextMock = new Mock<IApplicationDbContext>();
        _passwordHasherMock = new Mock<IPasswordHasher>();
        _tokenProviderMock = new Mock<ITokenProvider>();
        _handler = new LoginUserCommandHandler(_contextMock.Object, _passwordHasherMock.Object, _tokenProviderMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenUserNotFound()
    {
        // Arrange
        var command = new LoginUserCommand("nonexistent@test.com", "Password123");
        IQueryable<User> users = new List<User>().AsQueryable(); // Empty list, so SingleOrDefaultAsync will return null

        _contextMock.SetupGet(x => x.Users).ReturnsDbSet(users);

        // Act
        Result<string> result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(UserErrors.NotFoundByEmail, result.Error);
        _passwordHasherMock.Verify(x => x.Verify(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        _tokenProviderMock.Verify(x => x.Create(It.IsAny<User>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenPasswordIsIncorrect()
    {
        // Arrange
        var command = new LoginUserCommand("user@test.com", "IncorrectPassword");
        var existingUser = User.Create("user@test.com", "Test", "User", "hashedPassword"); // User with a hashed password
        IQueryable<User> users = new List<User> { existingUser }.AsQueryable();

        _contextMock.SetupGet(x => x.Users).ReturnsDbSet(users);
        _passwordHasherMock.Setup(x => x.Verify(command.Password, existingUser.PasswordHash)).Returns(false);

        // Act
        Result<string> result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(UserErrors.NotFoundByEmail, result.Error); // Handler returns NotFoundByEmail for incorrect password
        _tokenProviderMock.Verify(x => x.Create(It.IsAny<User>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenCredentialsAreCorrect()
    {
        // Arrange
        var command = new LoginUserCommand("user@test.com", "CorrectPassword");
        var existingUser = User.Create("user@test.com", "Test", "User", "hashedPassword");
        IQueryable<User> users = new List<User> { existingUser }.AsQueryable();
        string expectedToken = "someJwtToken";

        _contextMock.SetupGet(x => x.Users).ReturnsDbSet(users);
        _passwordHasherMock.Setup(x => x.Verify(command.Password, existingUser.PasswordHash)).Returns(true);
        _tokenProviderMock.Setup(x => x.Create(existingUser)).Returns(expectedToken);

        // Act
        Result<string> result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(expectedToken, result.Value);
        _tokenProviderMock.Verify(x => x.Create(existingUser), Times.Once);
    }
}
