using Application.Abstractions.Data;
using Application.Tasks.Create;

using Domain.Categories;

using Microsoft.EntityFrameworkCore;

using Moq;
using Moq.EntityFrameworkCore;

using SharedKernel;


namespace UnitTests.Application.Tasks.Create;

public class CreateTaskCommandHandlerTests
{
    private readonly Mock<IApplicationDbContext> _contextMock;
    private readonly CreateTaskCommandHandler _handler;

    public CreateTaskCommandHandlerTests()
    {
        _contextMock = new Mock<IApplicationDbContext>();
        _handler = new CreateTaskCommandHandler(_contextMock.Object);
    }

    [Fact]
    public async System.Threading.Tasks.Task Handle_ShouldReturnFailure_WhenCategoryDoesNotExist()
    {
        // Arrange
        var command = new CreateTaskCommand(
            Guid.NewGuid(),
            "Task Description",
            Guid.NewGuid(), // Non-existent category ID
            DateTime.UtcNow);

        IQueryable<Category> categories = new List<Category>().AsQueryable(); // Empty list, so AnyAsync for category will return false
        _contextMock.SetupGet(x => x.Categories).ReturnsDbSet(categories);

        // Explicitly mock DbSet<Domain.Tasks.Task>
        var mockTasksDbSet = new Mock<DbSet<Domain.Tasks.Task>>();
        _contextMock.Setup(x => x.Tasks).Returns(mockTasksDbSet.Object); // Return the mocked DbSet

        // Act
        Result<Guid> result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(CategoryErrors.NotFound(command.CategoryId), result.Error);
        mockTasksDbSet.Verify(x => x.Add(It.IsAny<Domain.Tasks.Task>()), Times.Never); // Verify on the DbSet mock
        _contextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async System.Threading.Tasks.Task Handle_ShouldReturnSuccess_WhenTaskIsCreatedSuccessfully()
    {
        // Arrange
        var categoryId = Guid.NewGuid();
        var command = new CreateTaskCommand(
            Guid.NewGuid(),
            "Task Description",
            categoryId,
            DateTime.UtcNow);

        var existingCategory = Category.Create("Existing Category", categoryId);
        IQueryable<Category> categories = new List<Category> { existingCategory }.AsQueryable();
        _contextMock.SetupGet(x => x.Categories).ReturnsDbSet(categories);

        var mockTasksDbSet = new Mock<DbSet<Domain.Tasks.Task>>();
        _contextMock.Setup(x => x.Tasks).Returns(mockTasksDbSet.Object);

        Domain.Tasks.Task capturedTask = null!; // Declare here
        mockTasksDbSet.Setup(x => x.Add(It.IsAny<Domain.Tasks.Task>()))
                      .Callback<Domain.Tasks.Task>(t => capturedTask = t); // Capture the argument

        _contextMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        // Act
        Result<Guid> result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotEqual(Guid.Empty, result.Value);

        mockTasksDbSet.Verify(x => x.Add(It.IsAny<Domain.Tasks.Task>()), Times.Once); // Verify Add was called
        Assert.NotNull(capturedTask); // Assert that the task was captured
        Assert.Equal(command.UserId, capturedTask.UserId);
        Assert.Equal(command.Description, capturedTask.Description);
        Assert.Equal(command.CategoryId, capturedTask.CategoryId);
        Assert.Equal(command.DueDate, capturedTask.DueDate);

        _contextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
