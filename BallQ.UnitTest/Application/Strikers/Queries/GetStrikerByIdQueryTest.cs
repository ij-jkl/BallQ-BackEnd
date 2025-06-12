namespace BallQ.UnitTest.Application.Strikers.Queries;

public class GetStrikerByIdQueryTest
{
    private readonly Mock<IStrikerRepository> _repositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GetStrikerByIdQueryHandler _handler;

    public GetStrikerByIdQueryTest()
    {
        _repositoryMock = new Mock<IStrikerRepository>();
        _mapperMock = new Mock<IMapper>();
        _handler = new GetStrikerByIdQueryHandler(_repositoryMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnOk_WhenStrikerExists()
    {
        // Arrange
        var striker = new StrikerEntity { Id = 1, Name = "Lionel Messi" };
        var strikerDto = new GetStrikerDto { Id = 1, Name = "Lionel Messi" };

        _repositoryMock.Setup(r => r.GetById(1)).ReturnsAsync(striker);
        _mapperMock.Setup(m => m.Map<GetStrikerDto>(striker)).Returns(strikerDto);

        var query = new GetStrikerByIdQuery { Id = 1 };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal((int)CodeHttp.OK, result.Code);
        Assert.NotNull(result.Response);
        Assert.Equal("Lionel Messi", ((GetStrikerDto)result.Response).Name);
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenStrikerDoesNotExist()
    {
        // Arrange
        _repositoryMock.Setup(r => r.GetById(2)).ReturnsAsync((StrikerEntity)null);

        var query = new GetStrikerByIdQuery { Id = 2 };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal((int)CodeHttp.NOTFOUND, result.Code);
        Assert.Null(result.Response);
        Assert.Contains("no striker with ID", result.Message);
    }

    [Fact]
    public async Task Handle_ShouldReturnInternalServerError_WhenExceptionThrown()
    {
        // Arrange
        _repositoryMock.Setup(r => r.GetById(It.IsAny<int>())).ThrowsAsync(new System.Exception("Database failure"));

        var query = new GetStrikerByIdQuery { Id = 3 };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal((int)CodeHttp.INTERNALSERVER, result.Code);
        Assert.Contains("Database failure", result.Message);
        Assert.Null(result.Response);
    }
}
