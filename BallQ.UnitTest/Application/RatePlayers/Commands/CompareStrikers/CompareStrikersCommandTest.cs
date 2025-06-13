namespace BallQ.UnitTest.Application.RatePlayers.Commands.CompareStrikers;

public class CompareStrikersCommandTest
{
    private readonly Mock<IPlayerRatingRepository> _ratingRepoMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly CompareStrikersCommandHandler _handler;

    public CompareStrikersCommandTest()
    {
        _ratingRepoMock = new Mock<IPlayerRatingRepository>();
        _mapperMock = new Mock<IMapper>();
        _handler = new CompareStrikersCommandHandler(_ratingRepoMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnComparisonResult_WhenIdsAreValid()
    {
        // Arrange
        var playerIds = new List<int> { 1, 2 };

        var ratings = new List<RatingEntity>
        {
            new RatingEntity
            {
                PlayerId = 1,
                GoalScore = 90,
                PassingScore = 85,
                ShootingScore = 88,
                InvolvementScore = 80,
                FinalScore = 86,
                Player = new StrikerEntity { Name = "Diego Valoyes" }
            },
            new RatingEntity
            {
                PlayerId = 2,
                GoalScore = 75,
                PassingScore = 70,
                ShootingScore = 78,
                InvolvementScore = 74,
                FinalScore = 74,
                Player = new StrikerEntity { Name = "Mario Ballotelli" }
            }
        };

        _ratingRepoMock
            .Setup(r => r.GetMultiplePlayerByIds(playerIds))
            .ReturnsAsync(ratings);
        
        // Act
        var command = new CompareStrikersCommand { PlayerIds = playerIds };
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal((int)CodeHttp.OK, result.Code);
        Assert.NotNull(result.Response);

        var comparisonList = Assert.IsType<List<RatingComparisonDto>>(result.Response);
        Assert.Equal(2, comparisonList.Count);
        Assert.Contains(comparisonList, r => r.PlayerId == 1 && r.Name == "Diego Valoyes");
        Assert.Contains(comparisonList, r => r.PlayerId == 2 && r.Name == "Mario Ballotelli");
    }

    [Fact]
    public async Task Handle_ShouldReturnInternalServerError_WhenExceptionThrown()
    {
        // Arrange
        _ratingRepoMock
            .Setup(r => r.GetMultiplePlayerByIds(It.IsAny<List<int>>()))
            .ThrowsAsync(new System.Exception("DB crash"));

        var command = new CompareStrikersCommand { PlayerIds = new List<int> { 1 } };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal((int)CodeHttp.INTERNALSERVER, result.Code);
        Assert.Contains("Exception during comparison", result.Message);
        Assert.Null(result.Response);
    }
    
    [Fact]
    public async Task Handle_ShouldFallbackToUnknownName_WhenStrikerNameIsNull()
    {
        // Arrange
        var playerIds = new List<int> { 1 };
        var ratings = new List<RatingEntity>
        {
            new RatingEntity
            {
                PlayerId = 1,
                FinalScore = 85,
                Player = null 
            }
        };

        _ratingRepoMock.Setup(r => r.GetMultiplePlayerByIds(playerIds)).ReturnsAsync(ratings);

        var command = new CompareStrikersCommand { PlayerIds = playerIds };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        var list = Assert.IsType<List<RatingComparisonDto>>(result.Response);
        Assert.Single(list);
        Assert.Equal("Unknown", list[0].Name);
    }
}
