namespace BallQ.UnitTest.Application.Strikers.Commands.UpdateStriker;

public class UpdateStrikerCommandTest
{
    private readonly Mock<IStrikerRepository> _repositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly UpdateStrikerCommandHandler _handler;

    public UpdateStrikerCommandTest()
    {
        _repositoryMock = new Mock<IStrikerRepository>();
        _mapperMock = new Mock<IMapper>();
        _handler = new UpdateStrikerCommandHandler(_repositoryMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnOk_WhenStrikerIsUpdated()
    {
        // Arrange
        var existingStriker = new StrikerEntity
        {
            Id = 1,
            Name = "Lamine Yamal",
            Age = 17
        };

        var updateDto = new UpdateStrikerDto
        {
            Id = 1,
            Name = "Luis Angulo",
            Age = 21
        };

        var updatedEntity = new StrikerEntity
        {
            Id = 1,
            Name = "Luis Angulo",
            Age = 21
        };

        _repositoryMock.Setup(r => r.GetById(1)).ReturnsAsync(existingStriker);
        _repositoryMock.Setup(r => r.Update(It.IsAny<StrikerEntity>())).ReturnsAsync(updatedEntity);
        _mapperMock.Setup(m => m.Map<UpdateStrikerDto>(updatedEntity)).Returns(updateDto);

        var command = new UpdateStrikerCommand { Striker = updateDto };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal((int)CodeHttp.OK, result.Code);
        Assert.Equal("The striker was updated successfully.", result.Message);
        Assert.Equal(updateDto.Name, ((UpdateStrikerDto)result.Response).Name);
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenStrikerDoesNotExist()
    {
        // Arrange
        _repositoryMock.Setup(r => r.GetById(99)).ReturnsAsync((StrikerEntity)null);

        var command = new UpdateStrikerCommand
        {
            Striker = new UpdateStrikerDto { Id = 99, Name = "Mauro Icardi" }
        };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal((int)CodeHttp.NOTFOUND, result.Code);
        Assert.Contains("does not exist", result.Message);
        Assert.Null(result.Response);
    }

    [Fact]
    public async Task Handle_ShouldReturnInternalServerError_WhenExceptionIsThrown()
    {
        // Arrange
        _repositoryMock.Setup(r => r.GetById(It.IsAny<int>())).ThrowsAsync(new Exception("Database failure"));

        var command = new UpdateStrikerCommand
        {
            Striker = new UpdateStrikerDto { Id = 1, Name = "Test" }
        };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal((int)CodeHttp.INTERNALSERVER, result.Code);
        Assert.Contains("An exception ocurred", result.Message);
        Assert.Null(result.Response);
    }
}
