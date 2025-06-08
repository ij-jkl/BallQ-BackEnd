namespace Domain.Entities;

public class RatingEntity
{
    public int Id { get; set; }
    public int PlayerId { get; set; }

    public double GoalScore { get; set; }
    public double PassingScore { get; set; }
    public double ShootingScore { get; set; }
    public double InvolvementScore { get; set; }
    public double FinalScore { get; set; }
    

    [JsonPropertyName("position")]
    public string Position { get; set; } = string.Empty;
    
    public StrikerEntity? Player { get; set; } 
}