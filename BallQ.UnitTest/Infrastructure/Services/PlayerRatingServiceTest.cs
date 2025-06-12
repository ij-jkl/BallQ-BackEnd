namespace BallQ.UnitTest.Infrastructure.Services;

public class PlayerRatingServiceTest
{
    private readonly Mock<IScoreCalculatorService<StrikerEntity>> _mockCalculator;
    private readonly PlayerRatingService<StrikerEntity, RatingEntity> _service;

    public PlayerRatingServiceTest()
    {
        _mockCalculator = new Mock<IScoreCalculatorService<StrikerEntity>>();
        _service = new PlayerRatingService<StrikerEntity, RatingEntity>(_mockCalculator.Object);
    }

    [Fact]
    public async Task GenerateStrikerRating_ShouldReturnCorrectRating()
    {
        // Arrange
        var striker = new StrikerEntity
        {
            Id = 1,
            Starts = 10,
            Appearances = 10,
            Position = "ST"
        };

        var allPlayers = new List<StrikerEntity> { striker };

        _mockCalculator.Setup(c => c.CalculateGoalScore(striker)).Returns(7);
        _mockCalculator.Setup(c => c.CalculateShootingScore(striker, allPlayers)).Returns(6);
        _mockCalculator.Setup(c => c.CalculatePassingScore(striker, allPlayers)).Returns(5);
        _mockCalculator.Setup(c => c.CalculateInvolvementScore(striker)).Returns(4);
        _mockCalculator.Setup(c => c.CalculateFinalScore(7, 6, 5, 4, 2)).Returns(6.05);

        // Act
        var result = await _service.GenerateStrikerRating(striker, allPlayers);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.PlayerId);
        Assert.Equal(7, result.GoalScore);
        Assert.Equal(6, result.ShootingScore);
        Assert.Equal(5, result.PassingScore);
        Assert.Equal(4, result.InvolvementScore);
        Assert.Equal(6.05, result.FinalScore);
        Assert.Equal("ST", result.Position);
    }

    [Fact]
    public async Task GenerateStrikerRating_ShouldThrow_WhenWrongGenericTypes()
    {
        // Arrange
        var wrongService = new PlayerRatingService<StrikerEntity, string>(_mockCalculator.Object);

        // Act & Assert
        await Assert.ThrowsAsync<UnsupportedPlayerTypeException>(() =>
            wrongService.GenerateStrikerRating(new StrikerEntity(), new List<StrikerEntity>()));
    }
    
    [Fact]
    public async Task GenerateStrikerRating_ShouldGiveZeroConsistency_WhenNoAppearances()
    {
        // Arrange
        var striker = new StrikerEntity
        {
            Id = 2,
            Starts = 5,
            Appearances = 0, // Evita división por cero
            Position = "ST"
        };

        var allPlayers = new List<StrikerEntity> { striker };

        _mockCalculator.Setup(c => c.CalculateGoalScore(striker)).Returns(6);
        _mockCalculator.Setup(c => c.CalculateShootingScore(striker, allPlayers)).Returns(5);
        _mockCalculator.Setup(c => c.CalculatePassingScore(striker, allPlayers)).Returns(4);
        _mockCalculator.Setup(c => c.CalculateInvolvementScore(striker)).Returns(3);
        _mockCalculator.Setup(c => c.CalculateFinalScore(6, 5, 4, 3, 0)).Returns(5.05);

        // Act
        var result = await _service.GenerateStrikerRating(striker, allPlayers);

        // Assert
        Assert.Equal(0, striker.Appearances);
        Assert.Equal(5.05, result.FinalScore);
    }

    [Fact]
    public async Task GenerateStrikerRating_ShouldHandleMultiplePlayersInList()
    {
        // Arrange
        var striker = new StrikerEntity
        {
            Id = 3,
            Starts = 9,
            Appearances = 10,
            Position = "ST"
        };

        var otherStriker = new StrikerEntity { Id = 99 };

        var allPlayers = new List<StrikerEntity> { striker, otherStriker };

        _mockCalculator.Setup(c => c.CalculateGoalScore(striker)).Returns(6);
        _mockCalculator.Setup(c => c.CalculateShootingScore(striker, allPlayers)).Returns(5);
        _mockCalculator.Setup(c => c.CalculatePassingScore(striker, allPlayers)).Returns(4);
        _mockCalculator.Setup(c => c.CalculateInvolvementScore(striker)).Returns(3);
        _mockCalculator.Setup(c => c.CalculateFinalScore(6, 5, 4, 3, 1.8)).Returns(5.4);

        // Act
        var result = await _service.GenerateStrikerRating(striker, allPlayers);

        // Assert
        Assert.Equal(3, result.InvolvementScore);
        Assert.Equal(1.8, striker.Starts / (double)striker.Appearances * 2, 2);
        Assert.Equal(5.4, result.FinalScore);
    }

    [Fact]
    public async Task GenerateStrikerRating_ShouldReturnCorrectPosition()
    {
        // Arrange
        var striker = new StrikerEntity
        {
            Id = 4,
            Starts = 8,
            Appearances = 10,
            Position = "CF"
        };

        var allPlayers = new List<StrikerEntity> { striker };

        _mockCalculator.Setup(c => c.CalculateGoalScore(It.IsAny<StrikerEntity>())).Returns(7);
        _mockCalculator.Setup(c => c.CalculateShootingScore(It.IsAny<StrikerEntity>(), allPlayers)).Returns(6);
        _mockCalculator.Setup(c => c.CalculatePassingScore(It.IsAny<StrikerEntity>(), allPlayers)).Returns(5);
        _mockCalculator.Setup(c => c.CalculateInvolvementScore(It.IsAny<StrikerEntity>())).Returns(4);
        _mockCalculator.Setup(c => c.CalculateFinalScore(7, 6, 5, 4, 1.6)).Returns(6.2);

        // Act
        var result = await _service.GenerateStrikerRating(striker, allPlayers);

        // Assert
        Assert.Equal("CF", result.Position);
    }

}
