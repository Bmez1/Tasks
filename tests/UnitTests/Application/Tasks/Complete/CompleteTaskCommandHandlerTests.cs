using Application.Abstractions.Data;
using Application.Tasks.Complete;

using Domain.Tasks;

using Moq;
using Moq.EntityFrameworkCore;

using SharedKernel;

using Task = Domain.Tasks.Task;

namespace UnitTests.Application.Tasks.Complete;

public class CompleteTaskCommandHandlerTests
{
    private readonly Mock<IApplicationDbContext> _contextMock;
    private readonly CompleteTaskCommandHandler _handler;

    public CompleteTaskCommandHandlerTests()
    {
        _contextMock = new Mock<IApplicationDbContext>();
        _handler = new CompleteTaskCommandHandler(_contextMock.Object);
    }

    [Fact]
    public async System.Threading.Tasks.Task Handle_ShouldReturnFailure_WhenTaskNotFound()
    {
        // Arrange
        var command = new CompleteTaskCommand(Guid.NewGuid(), Guid.NewGuid());
        IQueryable<Task> tasks = new List<Task>().AsQueryable();

        _contextMock.SetupGet(x => x.Tasks).ReturnsDbSet(tasks);

        // Act
        Result result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(TaskErrors.NotFound(command.TaskId), result.Error);
        _contextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async System.Threading.Tasks.Task Handle_ShouldReturnFailure_WhenTaskAlreadyCompleted()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var existingTask = Task.Create(userId, "name", "Description", Guid.NewGuid(), DateTime.UtcNow);
        existingTask.MarkComplete();
        IQueryable<Task> tasks = new List<Task> { existingTask }.AsQueryable();
        _contextMock.SetupGet(x => x.Tasks).ReturnsDbSet(tasks);
        var command = new CompleteTaskCommand(existingTask.Id, userId);

        // Act
        Result result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(TaskErrors.AlreadyCompleted, result.Error);
        _contextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async System.Threading.Tasks.Task Handle_ShouldReturnSuccess_WhenTaskIsCompletedSuccessfully()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var existingTask = Task.Create(userId, "name", "Description", Guid.NewGuid(), DateTime.UtcNow);
        Guid taskId = existingTask.Id;
        var command = new CompleteTaskCommand(taskId, userId);

        // Do not mark as completed
        IQueryable<Task> tasks = new List<Task> { existingTask }.AsQueryable();

        _contextMock.SetupGet(x => x.Tasks).ReturnsDbSet(tasks);
        _contextMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        // Act
        Result result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        _contextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        Assert.True(existingTask.IsCompleted);
        Assert.NotNull(existingTask.CompletedAt);
    }
}
