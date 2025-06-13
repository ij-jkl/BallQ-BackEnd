namespace BallQ.UnitTest.Application.Strikers.Commands.CreateStriker;

public class CreateStrikerCommandTest
{
    private readonly Mock<IStrikerRepository> _repositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly CreateStrikerCommandHandler _handler;

    public CreateStrikerCommandTest()
    {
        _repositoryMock = new Mock<IStrikerRepository>();
        _mapperMock = new Mock<IMapper>();
        _handler = new CreateStrikerCommandHandler(_repositoryMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnCreated_WhenStrikerIsCreated()
    {
        // Arrange
        var createDto = new CreateStrikerDto { Name = "Erling Haaland", Age = 23 };
        var entity = new StrikerEntity { Id = 1, Name = "Erling Haaland", Age = 23 };
        var savedEntity = new StrikerEntity { Id = 1, Name = "Erling Haaland", Age = 23 };

        _mapperMock.Setup(m => m.Map<StrikerEntity>(createDto)).Returns(entity);
        _repositoryMock.Setup(r => r.Create(entity)).ReturnsAsync(savedEntity);
        _mapperMock.Setup(m => m.Map<CreateStrikerDto>(savedEntity)).Returns(createDto);

        var command = new CreateStrikerCommand { Striker = createDto };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal((int)CodeHttp.CREATED, result.Code);
        Assert.Equal("Striker was created successfully.", result.Message);
        Assert.Equal("Erling Haaland", ((CreateStrikerDto)result.Response).Name);
    }

    [Fact]
    public async Task Handle_ShouldReturnInternalServerError_WhenExceptionThrown()
    {
        // Arrange
        _mapperMock.Setup(m => m.Map<StrikerEntity>(It.IsAny<CreateStrikerDto>()))
                   .Throws(new Exception("Mapping failed"));

        var command = new CreateStrikerCommand
        {
            Striker = new CreateStrikerDto { Name = "Kylian Mbappe", Age = 25 }
        };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal((int)CodeHttp.INTERNALSERVER, result.Code);
        Assert.Contains("Mapping failed", result.Message);
        Assert.Null(result.Response);
    }
}
