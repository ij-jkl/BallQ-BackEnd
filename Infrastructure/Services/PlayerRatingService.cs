namespace Infrastructure.Services;

public class PlayerRatingService<TPlayer, TRating> : IPlayerRatingService<TPlayer, TRating>
{
    private readonly IScoreCalculatorService<TPlayer> _calculator;

    public PlayerRatingService(IScoreCalculatorService<TPlayer> calculator)
    {
        _calculator = calculator;
    }

    public Task<TRating> GenerateStrikerRating(TPlayer player, List<TPlayer> allPlayers)
    {
        var goalScore = _calculator.CalculateGoalScore(player);
        var shootingScore = _calculator.CalculateShootingScore(player);
        var passingScore = _calculator.CalculatePassingScore(player);
        var involvementScore = _calculator.CalculateInvolvementScore(player);

        var consistencyBonus = GetConsistencyBonus(player);
        var finalScore = _calculator.CalculateFinalScore(goalScore, shootingScore, passingScore, involvementScore, consistencyBonus);
        
        if (typeof(TPlayer) == typeof(StrikerEntity) && typeof(TRating) == typeof(RatingEntity))
        {
            var striker = player as StrikerEntity;
            object rating = new RatingEntity
            {
                PlayerId = striker!.Id,
                GoalScore = goalScore,
                ShootingScore = shootingScore,
                PassingScore = passingScore,
                InvolvementScore = involvementScore,
                FinalScore = finalScore,
                Position = striker.Position
            };

            return Task.FromResult((TRating)rating);
        }

        throw new UnsupportedPlayerTypeException(typeof(TPlayer), typeof(TRating));
    }

    private double GetConsistencyBonus(TPlayer player)
    {
        if (player is StrikerEntity striker)
        {
            return striker.Appearances == 0 ? 0 : (double)striker.Starts / striker.Appearances * 2;
        }

        return 0;
    }
}