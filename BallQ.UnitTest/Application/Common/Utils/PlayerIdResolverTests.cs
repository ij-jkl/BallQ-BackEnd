namespace BallQ.UnitTest.Application.Common.Utils;

public class PlayerIdResolverTests
{
    private readonly Mock<IPlayerRatingRepository> _repoMock;

    public PlayerIdResolverTests()
    {
        _repoMock = new Mock<IPlayerRatingRepository>();
    }

    [Fact]
    public async Task ResolveFinalIdsTask_ShouldReturnIdsWithoutZerosUnchanged()
    {
        // Arrange
        var inputIds = new List<int> { 5, 10, 15 };

        // Act
        var result = await PlayerIdResolver.ResolveFinalIdsTask(inputIds, _repoMock.Object);

        // Assert
        Assert.Equal(3, result.Count);
        Assert.Contains(5, result);
        Assert.Contains(10, result);
        Assert.Contains(15, result);
    }

    [Fact]
    public async Task ResolveFinalIdsTask_ShouldReplaceZerosWithRandomPlayerIds()
    {
        // Arrange
        var inputIds = new List<int> { 0, 0 };

        _repoMock.SetupSequence(r => r.GetRandomPlayerRatings())
            .ReturnsAsync(new RatingEntity { PlayerId = 99 })
            .ReturnsAsync(new RatingEntity { PlayerId = 98 });

        // Act
        var result = await PlayerIdResolver.ResolveFinalIdsTask(inputIds, _repoMock.Object);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Contains(99, result);
        Assert.Contains(98, result);
    }

    [Fact]
    public async Task ResolveFinalIdsTask_ShouldNotAddDuplicateRandomIds()
    {
        // Arrange
        var inputIds = new List<int> { 0, 0 };

        _repoMock.SetupSequence(r => r.GetRandomPlayerRatings())
            .ReturnsAsync(new RatingEntity { PlayerId = 42 }) // First
            .ReturnsAsync(new RatingEntity { PlayerId = 42 }) // Retry
            .ReturnsAsync(new RatingEntity { PlayerId = 77 }); // Unique

        // Act
        var result = await PlayerIdResolver.ResolveFinalIdsTask(inputIds, _repoMock.Object);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Contains(42, result);
        Assert.Contains(77, result);
    }

    [Fact]
    public async Task ResolveFinalIdsTask_ShouldGiveUpAfterMaxRetries()
    {
        // Arrange
        var inputIds = new List<int> { 0 };

        // Always return the same ID, which will be considered duplicate
        _repoMock.Setup(r => r.GetRandomPlayerRatings())
            .ReturnsAsync(new RatingEntity { PlayerId = 1 });

        // Act
        var result = await PlayerIdResolver.ResolveFinalIdsTask(inputIds, _repoMock.Object, maxRetries: 3);

        // Assert
        // Only 1 attempt will be added because retries can't produce a unique one
        Assert.True(result.Count <= 1);
    }
}
