using Application.Abstractions.Data;
using Application.Tasks.Categories.Create;

using Domain.Categories;

using Microsoft.EntityFrameworkCore;

using Moq;
using Moq.EntityFrameworkCore;

using SharedKernel;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Xunit;

namespace UnitTests.Application.Categories.Create;

public class CreateCategoryCommandHandlerTests
{
    private readonly Mock<IApplicationDbContext> _contextMock;
    private readonly CreateCategoryCommandHandler _handler;

    public CreateCategoryCommandHandlerTests()
    {
        _contextMock = new Mock<IApplicationDbContext>();
        _handler = new CreateCategoryCommandHandler(_contextMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenCategoryIsCreatedSuccessfully()
    {
        // Arrange
        var command = new CreateCategoryCommand("TestCategory");
        var categories = new List<Category>(); // Empty list for initial DbSet

        _contextMock.SetupGet(x => x.Categories).ReturnsDbSet(categories);
        _contextMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1); // Simulate 1 entity saved

        // Act
        Result<Guid> result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotEqual(Guid.Empty, result.Value);
        _contextMock.Verify(x => x.Categories.Add(It.Is<Category>(c => c.Name == command.Name)), Times.Once);
        _contextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
