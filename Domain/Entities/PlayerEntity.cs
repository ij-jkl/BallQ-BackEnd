namespace Domain.Entities;

public class PlayerEntity
{
    public int Id { get; set; }
    
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("position")]
    public string Position { get; set; } = string.Empty;

    [JsonPropertyName("club")]
    public string Club { get; set; } = string.Empty;

    [JsonPropertyName("nationality")]
    public string Nationality { get; set; } = string.Empty;

    [JsonPropertyName("height")]
    public string Height { get; set; } = string.Empty;

    [JsonPropertyName("weight")]
    public string Weight { get; set; } = string.Empty;

    [JsonPropertyName("preferred_foot")]
    public string PreferredFoot { get; set; } = string.Empty;

    [JsonPropertyName("age")]
    public int Age { get; set; }

    [JsonPropertyName("appearances")]
    public string Appearances { get; set; } = string.Empty;

    [JsonPropertyName("starts")]
    public int Starts { get; set; }

    [JsonPropertyName("minutes_played")]
    public int MinutesPlayed { get; set; }

    [JsonPropertyName("goals")]
    public int Goals { get; set; }

    [JsonPropertyName("shots")]
    public int Shots { get; set; }

    [JsonPropertyName("shots_on_target")]
    public int ShotsOnTarget { get; set; }

    [JsonPropertyName("assists")]
    public int Assists { get; set; }

    [JsonPropertyName("key_passes_created")]
    public int KeyPassesCreated { get; set; }

    [JsonPropertyName("passes_completed")]
    public int PassesCompleted { get; set; }

    [JsonPropertyName("market_value")]
    public string MarketValue { get; set; } = string.Empty;

    [JsonPropertyName("dribbles_made")]
    public int DribblesMade { get; set; }

    [JsonPropertyName("goals_per_90")]
    public decimal GoalsPer90 { get; set; }

    [JsonPropertyName("shots_on_target_per_90")]
    public decimal ShotsOnTargetPer90 { get; set; }

    [JsonPropertyName("assists_per_90")]
    public decimal AssistsPer90 { get; set; }

    [JsonPropertyName("key_passes_created_per_90")]
    public decimal KeyPassesCreatedPer90 { get; set; }

    [JsonPropertyName("passes_completed_per_90")]
    public decimal PassesCompletedPer90 { get; set; }

    [JsonPropertyName("conversion_rate")]
    public decimal ConversionRate { get; set; }

    [JsonPropertyName("shot_accuracy")]
    public decimal ShotAccuracy { get; set; }

    [JsonPropertyName("goal_involvement_per_90")]
    public decimal GoalInvolvementPer90 { get; set; }

    [JsonPropertyName("rating")]
    public decimal Rating { get; set; }

    [JsonPropertyName("normalized_rating")]
    public decimal NormalizedRating { get; set; }

    [JsonIgnore]
    public PlayerPositionCategory PositionCategory =>
        Position.Contains("ST") ? PlayerPositionCategory.Striker :
        Position.Contains("M") ? PlayerPositionCategory.Midfielder :
        Position.Contains("D") ? PlayerPositionCategory.Defender :
        Position.Contains("GK") ? PlayerPositionCategory.Goalkeeper :
        PlayerPositionCategory.Unknown;
}