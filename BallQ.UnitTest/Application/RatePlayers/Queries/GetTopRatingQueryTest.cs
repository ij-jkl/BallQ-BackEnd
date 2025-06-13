namespace BallQ.UnitTest.Application.Queries.RatePlayers;

public class GetTopRatingQueryTest
{
    private readonly Mock<IPlayerRatingRepository> _ratingRepoMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GetTopRatingsQueryHandler _handler;

    public GetTopRatingQueryTest()
    {
        _ratingRepoMock = new Mock<IPlayerRatingRepository>();
        _mapperMock = new Mock<IMapper>();
        _handler = new GetTopRatingsQueryHandler(_ratingRepoMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnTopRatingsSuccessfully()
    {
        // Arrange
        var query = new GetTopRatingsQuery { Limit = 2, Ascending = false };

        var ratingEntities = new List<RatingEntity>
        {
            new RatingEntity { Id = 1, FinalScore = 95.5, Position = "Striker" },
            new RatingEntity { Id = 2, FinalScore = 90.2, Position = "Striker" }
        };

        var ratingDtos = new List<GetRatingDto>
        {
            new GetRatingDto { Id = 1, FinalScore = 95.5, Position = "Striker" },
            new GetRatingDto { Id = 2, FinalScore = 90.2, Position = "Striker" }
        };

        _ratingRepoMock
            .Setup(r => r.GetTopRatingsAsync(2, false, It.IsAny<CancellationToken>()))
            .ReturnsAsync(ratingEntities);

        _mapperMock
            .Setup(m => m.Map<List<GetRatingDto>>(ratingEntities))
            .Returns(ratingDtos);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Deserialize anonymous response into matching structure
        var json = JsonSerializer.Serialize(result.Response);
        var anonymousTemplate = new
        {
            Items = new List<GetRatingDto>(),
            Metadata = new
            {
                TotalItems = 0,
                Limit = 0,
                Order = ""
            }
        };

        var response = JsonSerializer.Deserialize(json, anonymousTemplate.GetType())!;
        var metadata = response.GetType().GetProperty("Metadata")!.GetValue(response)!;
        var items = (List<GetRatingDto>)response.GetType().GetProperty("Items")!.GetValue(response)!;

        // Assert
        Assert.Equal(200, result.Code);
        Assert.Equal(2, (int)metadata.GetType().GetProperty("TotalItems")!.GetValue(metadata)!);
        Assert.Equal("desc", (string)metadata.GetType().GetProperty("Order")!.GetValue(metadata)!);
        Assert.Equal(95.5, items[0].FinalScore);
        Assert.Equal("Striker", items[0].Position);
    }

    [Fact]
    public async Task Handle_ShouldReturnInternalServerError_WhenExceptionIsThrown()
    {
        // Arrange
        _ratingRepoMock
            .Setup(r => r.GetTopRatingsAsync(It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Rating DB error"));

        var query = new GetTopRatingsQuery { Limit = 5, Ascending = true };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal((int)CodeHttp.INTERNALSERVER, result.Code);
        Assert.Contains("Rating DB error", result.Message);
        Assert.Null(result.Response);
    }

    [Fact]
    public async Task Handle_ShouldReturnRatingsSortedDescending_WhenDescendingIsRequested()
    {
        // Arrange
        var query = new GetTopRatingsQuery { Limit = 3, Ascending = false };

        var ratingEntities = new List<RatingEntity>
        {
            new RatingEntity { Id = 1, FinalScore = 88.0 },
            new RatingEntity { Id = 2, FinalScore = 95.0 },
            new RatingEntity { Id = 3, FinalScore = 90.0 }
        };

        var sortedEntities = ratingEntities.OrderByDescending(r => r.FinalScore).ToList();

        _ratingRepoMock
            .Setup(r => r.GetTopRatingsAsync(3, false, It.IsAny<CancellationToken>()))
            .ReturnsAsync(sortedEntities);

        var ratingDtos = sortedEntities
            .Select(r => new GetRatingDto { Id = r.Id, FinalScore = r.FinalScore })
            .ToList();

        _mapperMock
            .Setup(m => m.Map<List<GetRatingDto>>(It.IsAny<List<RatingEntity>>()))
            .Returns(ratingDtos);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Deserialize and verify
        var json = JsonSerializer.Serialize(result.Response);
        var anonymousTemplate = new
        {
            Items = new List<GetRatingDto>(),
            Metadata = new { TotalItems = 0, Limit = 0, Order = "" }
        };

        var response = JsonSerializer.Deserialize(json, anonymousTemplate.GetType())!;
        var metadata = response.GetType().GetProperty("Metadata")!.GetValue(response)!;
        var items = (List<GetRatingDto>)response.GetType().GetProperty("Items")!.GetValue(response)!;

        // Assert
        Assert.Equal(3, items.Count);
        Assert.True(items[0].FinalScore >= items[1].FinalScore);
        Assert.True(items[1].FinalScore >= items[2].FinalScore);
        Assert.Equal(95.0, items[0].FinalScore);
        Assert.Equal("desc", (string)metadata.GetType().GetProperty("Order")!.GetValue(metadata)!);
    }
}
