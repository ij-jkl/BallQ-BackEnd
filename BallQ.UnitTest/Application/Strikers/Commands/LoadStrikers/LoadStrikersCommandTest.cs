namespace BallQ.UnitTest.Application.Strikers.Commands.LoadStrikers;

public class LoadStrikersCommandTest
{
    private readonly Mock<IDataLoadRepository> _repositoryMock;
    private readonly LoadStrikersCommandHandler _handler;

    public LoadStrikersCommandTest()
    {
        _repositoryMock = new Mock<IDataLoadRepository>();
        _handler = new LoadStrikersCommandHandler(_repositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnOk_WhenScriptExecutesSuccessfully()
    {
        // Arrange
        _repositoryMock.Setup(r => r.ExecuteStrikerInsertScriptAsync())
            .ReturnsAsync(true);

        var command = new LoadStrikersCommand();

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal((int)CodeHttp.OK, result.Code);
        Assert.Equal("Strikers inserted successfully.", result.Message);
        Assert.Null(result.Response);
    }

    [Fact]
    public async Task Handle_ShouldReturnConflict_WhenScriptExecutionFails()
    {
        // Arrange
        _repositoryMock.Setup(r => r.ExecuteStrikerInsertScriptAsync())
            .ReturnsAsync(false);

        var command = new LoadStrikersCommand();

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal((int)CodeHttp.CONFLICT, result.Code);
        Assert.Equal("Script execution failed.", result.Message);
        Assert.Null(result.Response);
    }

    [Fact]
    public async Task Handle_ShouldReturnInternalServerError_WhenExceptionThrown()
    {
        // Arrange
        _repositoryMock.Setup(r => r.ExecuteStrikerInsertScriptAsync())
            .ThrowsAsync(new Exception("Database access error"));

        var command = new LoadStrikersCommand();

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal((int)CodeHttp.INTERNALSERVER, result.Code);
        Assert.Contains("Database access error", result.Message);
        Assert.Null(result.Response);
    }
}
