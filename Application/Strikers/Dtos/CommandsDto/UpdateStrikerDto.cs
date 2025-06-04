namespace Application.Strikers.Dtos.CommandsDto;

public class UpdateStrikerDto
{
    public int Id { get; set; } 
    public string? Name { get; set; }
    public string? Position { get; set; }
    public string? Club { get; set; }
    public string? Nationality { get; set; }
    public double? HeightCm { get; set; }
    public double? WeightKg { get; set; }
    public string? PreferredFoot { get; set; }
    public int? Age { get; set; }
    public int? Appearances { get; set; }
    public int? Subs { get; set; }
    public int? Starts { get; set; }
    public int? MinutesPlayed { get; set; }
    public int? Goals { get; set; }
    public int? Shots { get; set; }
    public int? ShotsOnTarget { get; set; }
    public int? Assists { get; set; }
    public int? KeyPasses { get; set; }
    public int? PassesCompleted { get; set; }
    public long? MarketValueMin { get; set; }
    public long? MarketValueMax { get; set; }
    public int? DribblesMade { get; set; }
    public decimal? GoalsPer90 { get; set; }
    public decimal? ShotsOnTargetPer90 { get; set; }
    public decimal? AssistsPer90 { get; set; }
    public decimal? KeyPassesPer90 { get; set; }
    public decimal? PassesCompletedPer90 { get; set; }
    public decimal? ConversionRate { get; set; }
    public decimal? ShotAccuracy { get; set; }
    public decimal? GoalInvolvementPer90 { get; set; }
}