namespace Domain.Entities;

public class StrikerRating
{
    public int Id { get; set; }
    public int StrikerId { get; set; }
    public double GoalScore { get; set; }
    public double PassingScore { get; set; }
    public double ShootingScore { get; set; }
    public double InvolvementScore { get; set; }
    public double FinalScore { get; set; }

    public StrikerEntity Striker { get; set; }
}