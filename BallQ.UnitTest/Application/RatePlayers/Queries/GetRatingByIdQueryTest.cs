namespace BallQ.UnitTest.Application.Queries.RatePlayers;

public class GetRatingByIdQueryTest
{
    private readonly Mock<IPlayerRatingRepository> _ratingRepoMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GetRatingByIdQueryHandler _handler;

    public GetRatingByIdQueryTest()
    {
        _ratingRepoMock = new Mock<IPlayerRatingRepository>();
        _mapperMock = new Mock<IMapper>();
        _handler = new GetRatingByIdQueryHandler(_ratingRepoMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnOk_WhenRatingExists()
    {
        // Arrange
        var ratingId = 1;
        var entity = new RatingEntity
        {
            Id = ratingId,
            FinalScore = 92.3,
            Position = "Striker"
        };

        var dto = new GetRatingDto
        {
            Id = ratingId,
            FinalScore = 92.3,
            Position = "Striker"
        };

        _ratingRepoMock.Setup(r => r.GetById(ratingId)).ReturnsAsync(entity);
        _mapperMock.Setup(m => m.Map<GetRatingDto>(entity)).Returns(dto);

        var query = new GetRatingByIdQuery { Id = ratingId };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(200, result.Code);
        Assert.Equal($"The rating with the following ID ({ratingId}) is : .", result.Message);
        Assert.NotNull(result.Response);

        var returnedDto = Assert.IsType<GetRatingDto>(result.Response);
        Assert.Equal(dto.Id, returnedDto.Id);
        Assert.Equal(dto.FinalScore, returnedDto.FinalScore);
        Assert.Equal(dto.Position, returnedDto.Position);
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenRatingDoesNotExist()
    {
        // Arrange
        var queryId = 999;

        _ratingRepoMock.Setup(r => r.GetById(queryId)).ReturnsAsync((RatingEntity)null);

        var query = new GetRatingByIdQuery { Id = queryId };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal((int)CodeHttp.NOTFOUND, result.Code);
        Assert.Equal($"The rating with ID is : ({queryId})", result.Message);
        Assert.Null(result.Response);
    }

    [Fact]
    public async Task Handle_ShouldReturnInternalServerError_WhenExceptionThrown()
    {
        // Arrange
        _ratingRepoMock.Setup(r => r.GetById(It.IsAny<int>())).ThrowsAsync(new Exception("DB crashed"));

        var query = new GetRatingByIdQuery { Id = 5 };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal((int)CodeHttp.INTERNALSERVER, result.Code);
        Assert.Contains("Error getting rating by ID", result.Message);
        Assert.Contains("DB crashed", result.Message);
        Assert.Null(result.Response);
    }
}
