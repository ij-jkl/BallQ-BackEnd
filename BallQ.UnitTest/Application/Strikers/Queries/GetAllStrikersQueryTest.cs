namespace BallQ.UnitTest.Application.Strikers.Queries;

public class GetAllStrikersQueryTest
{
    private readonly Mock<IStrikerRepository> _repositoryMock;
    private readonly Mock<IPaginationService> _paginationServiceMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GetAllStrikersQueryHandler _handler;

    public GetAllStrikersQueryTest()
    {
        _repositoryMock = new Mock<IStrikerRepository>();
        _paginationServiceMock = new Mock<IPaginationService>();
        _mapperMock = new Mock<IMapper>();
        _handler = new GetAllStrikersQueryHandler(_repositoryMock.Object, _paginationServiceMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnOk_WhenStrikersExist()
    {
        // Arrange
        var strikerDtos = new List<GetStrikerDto>
        {
            new GetStrikerDto { Id = 1, Name = "John Doe" },
            new GetStrikerDto { Id = 2, Name = "Leo Messi" }
        };

        var paginationResult = new PaginationDto<GetStrikerDto>
        {
            Items = strikerDtos,
            Metadata = new PaginationData
            {
                CurrentPage = 1,
                PageSize = 5,
                TotalItems = 2,
                TotalPages = 1
            }
        };

        _repositoryMock.Setup(r => r.AsQueryable()).Returns(new List<StrikerEntity>().AsQueryable());

        _paginationServiceMock
            .Setup(p => p.PaginateAsync(
                It.IsAny<IQueryable<StrikerEntity>>(),
                1,
                5,
                It.IsAny<Func<StrikerEntity, GetStrikerDto>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(paginationResult);

        var query = new GetAllStrikersQuery { PageNumber = 1, PageSize = 5 };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal((int)CodeHttp.OK, result.Code);
        Assert.NotNull(result.Response);
        Assert.Contains("Strikers list", result.Message);
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenNoStrikersExist()
    {
        // Arrange
        var emptyPagination = new PaginationDto<GetStrikerDto>
        {
            Items = new List<GetStrikerDto>(),
            Metadata = new PaginationData
            {
                CurrentPage = 1,
                PageSize = 5,
                TotalItems = 0,
                TotalPages = 0
            }
        };

        _repositoryMock.Setup(r => r.AsQueryable()).Returns(new List<StrikerEntity>().AsQueryable());

        _paginationServiceMock
            .Setup(p => p.PaginateAsync(
                It.IsAny<IQueryable<StrikerEntity>>(),
                1,
                5,
                It.IsAny<Func<StrikerEntity, GetStrikerDto>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(emptyPagination);

        var query = new GetAllStrikersQuery { PageNumber = 1, PageSize = 5 };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal((int)CodeHttp.NOTFOUND, result.Code);
        Assert.Null(result.Response);
        Assert.Contains("No strikers", result.Message);
    }

    [Fact]
    public async Task Handle_ShouldReturnInternalServerError_WhenExceptionThrown()
    {
        // Arrange
        _repositoryMock.Setup(r => r.AsQueryable()).Throws(new System.Exception("Simulated DB Error"));

        var query = new GetAllStrikersQuery { PageNumber = 1, PageSize = 5 };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal((int)CodeHttp.INTERNALSERVER, result.Code);
        Assert.Null(result.Response);
        Assert.Contains("Error fetching striker list", result.Message);
    }
}
