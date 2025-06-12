namespace Application.RatePlayers.Dtos.QueriesDto;

public class RatingComparisonDto
{
    public int PlayerId { get; set; }
    public string Name { get; set; } = string.Empty;
    public double GoalScore { get; set; }
    public double PassingScore { get; set; }
    public double ShootingScore { get; set; }
    public double InvolvementScore { get; set; }
    public double FinalScore { get; set; }
}
