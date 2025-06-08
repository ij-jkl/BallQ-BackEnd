namespace Infrastructure.Services;

public class PlayerScoreCalculator<TPlayer> : IScoreCalculatorService<TPlayer>
{
    private readonly IStatNormalizerService _normalizer;

    public PlayerScoreCalculator(IStatNormalizerService normalizer)
    {
        _normalizer = normalizer;
    }

    public double CalculateGoalScore(TPlayer player)
    {
        if (player is StrikerEntity s)
            return _normalizer.NormalizeMinMax((double)s.GoalsPer90, 0, 1);

        throw new UnsupportedScoreCalculationException("GoalScore", typeof(TPlayer));
    }
    
    public double CalculateShootingScore(TPlayer player, List<TPlayer> allPlayers)
    {
        if (player is StrikerEntity s)
        {
            var values = allPlayers.OfType<StrikerEntity>()
                .Select(p => (double)(p.ShotsOnTargetPer90 * 0.6m + p.ConversionRate * 0.4m))
                .ToList();

            var current = (double)(s.ShotsOnTargetPer90 * 0.6m + s.ConversionRate * 0.4m);
            return _normalizer.NormalizeMinMax(current, values.Min(), values.Max());
        }

        throw new UnsupportedScoreCalculationException("ShootingScore", typeof(TPlayer));
    }
    
    public double CalculatePassingScore(TPlayer player, List<TPlayer> allPlayers)
    {
        if (player is StrikerEntity s)
        {
            var values = allPlayers.OfType<StrikerEntity>()
                .Select(p => (double)(p.PassesCompletedPer90 * 0.6m + p.AssistsPer90 * 0.4m))
                .ToList();

            var current = (double)(s.PassesCompletedPer90 * 0.6m + s.AssistsPer90 * 0.4m);
            return _normalizer.NormalizeMinMax(current, values.Min(), values.Max());
        }

        throw new UnsupportedScoreCalculationException("PassingScore", typeof(TPlayer));
    }


    public double CalculateInvolvementScore(TPlayer player)
    {
        if (player is StrikerEntity s)
        {
            var dribblesPer90 = s.MinutesPlayed == 0 ? 0 : s.DribblesMade / (s.MinutesPlayed / 90.0);
            return _normalizer.NormalizeMinMax(
                dribblesPer90 * 2 +
                (double)s.KeyPassesPer90 * 2 +
                (double)s.GoalInvolvementPer90 * 4 +
                (s.MinutesPlayed / 90.0) * 1,
                0, 50
            );
        }

        throw new UnsupportedScoreCalculationException("InvolvementScore", typeof(TPlayer));
    }

    public double CalculateFinalScore(double goal, double shooting, double passing, double involvement, double consistencyBonus)
    {
        return goal * 0.35 +
               shooting * 0.30 +
               passing * 0.20 +
               involvement * 0.10 +
               consistencyBonus * 0.05;
    }
}
