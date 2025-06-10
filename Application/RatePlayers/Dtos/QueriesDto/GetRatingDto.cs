namespace Application.RatePlayers.Dtos.QueriesDto;

public class GetRatingDto
{
    public int Id { get; set; }
    public int PlayerId { get; set; }

    public double GoalScore { get; set; }
    public double PassingScore { get; set; }
    public double ShootingScore { get; set; }
    public double InvolvementScore { get; set; }
    public double FinalScore { get; set; }

    public string Position { get; set; } = string.Empty;

    public string? PlayerName { get; set; }
    public string? Nationality { get; set; }
}
