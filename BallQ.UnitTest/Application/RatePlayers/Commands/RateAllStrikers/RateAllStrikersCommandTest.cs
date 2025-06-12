namespace BallQ.UnitTest.Application.RatePlayers.Commands.RateAllStrikers;

public class RateAllStrikersCommandTest
{
    private readonly Mock<IStrikerRepository> _strikerRepoMock;
    private readonly Mock<IPlayerRatingRepository> _ratingRepoMock;
    private readonly Mock<IPlayerRatingService<StrikerEntity, RatingEntity>> _ratingServiceMock;
    private readonly RateAllStrikersCommandHandler _handler;

    public RateAllStrikersCommandTest()
    {
        _strikerRepoMock = new Mock<IStrikerRepository>();
        _ratingRepoMock = new Mock<IPlayerRatingRepository>();
        _ratingServiceMock = new Mock<IPlayerRatingService<StrikerEntity, RatingEntity>>();
        _handler = new RateAllStrikersCommandHandler(_strikerRepoMock.Object, _ratingRepoMock.Object, _ratingServiceMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnOk_WhenRatingsAreCalculated()
    {
        // Arrange
        var strikers = new List<StrikerEntity>
        {
            new StrikerEntity { Id = 1, Name = "Leo Messi" },
            new StrikerEntity { Id = 2, Name = "Dayro Moreno" }
        };

        _strikerRepoMock.Setup(r => r.GetAllStrikers()).ReturnsAsync(strikers);

        _ratingServiceMock
            .Setup(r => r.GenerateStrikerRating(It.IsAny<StrikerEntity>(), strikers))
            .ReturnsAsync((StrikerEntity striker, List<StrikerEntity> all) => 
                new RatingEntity { PlayerId = striker.Id, FinalScore = 85.0 });

        _ratingRepoMock.Setup(r => r.SaveAllStrikers(It.IsAny<List<RatingEntity>>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(new RateAllStrikersCommand(), CancellationToken.None);

        // Assert
        Assert.Equal(200, result.Code);
        Assert.Equal("Ratings calculated and upserted successfully.", result.Message);
        Assert.Null(result.Response);
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenNoStrikersExist()
    {
        // Arrange
        _strikerRepoMock.Setup(r => r.GetAllStrikers()).ReturnsAsync(new List<StrikerEntity>());

        // Act
        var result = await _handler.Handle(new RateAllStrikersCommand(), CancellationToken.None);

        // Assert
        Assert.Equal((int)CodeHttp.NOTFOUND, result.Code);
        Assert.Contains("No strikers found", result.Message);
        Assert.Null(result.Response);
    }

    [Fact]
    public async Task Handle_ShouldReturnInternalServerError_WhenExceptionThrown()
    {
        // Arrange
        _strikerRepoMock.Setup(r => r.GetAllStrikers()).ThrowsAsync(new Exception("DB Crash"));

        // Act
        var result = await _handler.Handle(new RateAllStrikersCommand(), CancellationToken.None);

        // Assert
        Assert.Equal(500, result.Code);
        Assert.Contains("DB Crash", result.Message);
        Assert.Null(result.Response);
    }
}
