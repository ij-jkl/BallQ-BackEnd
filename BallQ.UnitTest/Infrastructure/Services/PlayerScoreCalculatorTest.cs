namespace BallQ.UnitTest.Infrastructure.Services
{
    public class PlayerScoreCalculatorTests
    {
        private readonly PlayerScoreCalculator<StrikerEntity> _calculator;
        private readonly Mock<IStatNormalizerService> _mockNormalizer;

        public PlayerScoreCalculatorTests()
        {
            _mockNormalizer = new Mock<IStatNormalizerService>();
            _calculator = new PlayerScoreCalculator<StrikerEntity>(_mockNormalizer.Object);
        }

        [Fact]
        public void CalculateGoalScore_ShouldReturnNormalizedValue()
        {
            var player = new StrikerEntity { GoalsPer90 = 0.7m };
            _mockNormalizer.Setup(n => n.NormalizeMinMax(0.7, 0, 1)).Returns(7);

            var result = _calculator.CalculateGoalScore(player);

            Assert.Equal(7, result);
        }

        [Fact]
        public void CalculateShootingScore_ShouldUseCorrectFormulaAndNormalize()
        {
            var player = new StrikerEntity { ShotsOnTargetPer90 = 2.0m, ConversionRate = 0.4m };
            var all = new List<StrikerEntity>
            {
                player,
                new StrikerEntity { ShotsOnTargetPer90 = 1.0m, ConversionRate = 0.2m },
                new StrikerEntity { ShotsOnTargetPer90 = 3.0m, ConversionRate = 0.6m },
            };

            var weighted = (double)(2.0m * 0.6m + 0.4m * 0.4m);
            _mockNormalizer.Setup(n => n.NormalizeMinMax(weighted, It.IsAny<double>(), It.IsAny<double>())).Returns(5);

            var result = _calculator.CalculateShootingScore(player, all);
            Assert.Equal(5, result);
        }

        [Fact]
        public void CalculatePassingScore_ShouldUseCorrectFormulaAndNormalize()
        {
            var player = new StrikerEntity { PassesCompletedPer90 = 30m, AssistsPer90 = 0.5m };
            var all = new List<StrikerEntity>
            {
                player,
                new StrikerEntity { PassesCompletedPer90 = 25m, AssistsPer90 = 0.4m },
                new StrikerEntity { PassesCompletedPer90 = 35m, AssistsPer90 = 0.6m },
            };

            var weighted = (double)(30m * 0.6m + 0.5m * 0.4m);
            _mockNormalizer.Setup(n => n.NormalizeMinMax(weighted, It.IsAny<double>(), It.IsAny<double>())).Returns(6);

            var result = _calculator.CalculatePassingScore(player, all);
            Assert.Equal(6, result);
        }

        [Fact]
        public void CalculateInvolvementScore_ShouldUseCorrectFormulaAndNormalize()
        {
            var player = new StrikerEntity
            {
                MinutesPlayed = 900,
                DribblesMade = 20,
                KeyPassesPer90 = 1.5m,
                GoalInvolvementPer90 = 0.8m
            };

            var dribblesPer90 = 20 / (900 / 90.0);
            var expected = dribblesPer90 * 2 + 1.5 * 2 + 0.8 * 4 + 10;
            _mockNormalizer.Setup(n => n.NormalizeMinMax(expected, 0, 50)).Returns(8);

            var result = _calculator.CalculateInvolvementScore(player);
            Assert.Equal(8, result);
        }

        [Fact]
        public void CalculateFinalScore_ShouldWeightAndSumCorrectly()
        {
            var result = _calculator.CalculateFinalScore(goal: 7, shooting: 8, passing: 6, involvement: 5, consistencyBonus: 2);

            var expected = 7 * 0.35 + 8 * 0.30 + 6 * 0.20 + 5 * 0.10 + 2 * 0.05;
            Assert.Equal(expected, result);
        }
    }
}
