namespace BallQ.UnitTest.Application.Queries.RatePlayers;

public class GetAllRatingsQueryTest
{
    private readonly Mock<IPlayerRatingRepository> _ratingRepoMock;
    private readonly Mock<IPaginationService> _paginationServiceMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GetAllRatingsQueryHandler _handler;

    public GetAllRatingsQueryTest()
    {
        _ratingRepoMock = new Mock<IPlayerRatingRepository>();
        _paginationServiceMock = new Mock<IPaginationService>();
        _mapperMock = new Mock<IMapper>();
        _handler = new GetAllRatingsQueryHandler(_ratingRepoMock.Object, _paginationServiceMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnPaginatedRatings_WhenDataExists()
    {
        // Arrange
        var ratingDtos = new List<GetRatingDto>
        {
            new GetRatingDto { Id = 1, FinalScore = 91.0, Position = "Striker" },
            new GetRatingDto { Id = 2, FinalScore = 87.5, Position = "Striker" }
        };

        var paginationDto = new PaginationDto<GetRatingDto>
        {
            Items = ratingDtos,
            Metadata = new PaginationData
            {
                CurrentPage = 1,
                PageSize = 5,
                TotalItems = 2,
                TotalPages = 1
            }
        };

        _ratingRepoMock.Setup(r => r.AsQueryable())
            .Returns(new List<RatingEntity>().AsQueryable());

        _paginationServiceMock.Setup(p => p.PaginateAsync(
            It.IsAny<IQueryable<RatingEntity>>(),
            1,
            5,
            It.IsAny<Func<RatingEntity, GetRatingDto>>(),
            It.IsAny<CancellationToken>()
        )).ReturnsAsync(paginationDto);

        var query = new GetAllRatingsQuery { PageNumber = 1, PageSize = 5 };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal((int)CodeHttp.OK, result.Code);
        Assert.Equal("The list of ratings was successfully retrieved.", result.Message);
        Assert.NotNull(result.Response);

        var response = Assert.IsType<PaginationDto<GetRatingDto>>(result.Response);
        Assert.Equal(2, response.Items.Count);
        Assert.Equal(1, response.Metadata.CurrentPage);
    }

    [Fact]
    public async Task Handle_ShouldReturnInternalServerError_WhenExceptionIsThrown()
    {
        // Arrange
        _ratingRepoMock.Setup(r => r.AsQueryable())
            .Throws(new Exception("Simulated DB error"));

        var query = new GetAllRatingsQuery { PageNumber = 1, PageSize = 5 };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal((int)CodeHttp.INTERNALSERVER, result.Code);
        Assert.Contains("Error fetching striker list", result.Message);
        Assert.Null(result.Response);
    }
}
