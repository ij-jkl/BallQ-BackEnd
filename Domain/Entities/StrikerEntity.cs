namespace Domain.Entities
{
    public class StrikerEntity
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

        [JsonPropertyName("height_cm")]
        public double HeightCm { get; set; }

        [JsonPropertyName("weight_kg")]
        public double WeightKg { get; set; }

        [JsonPropertyName("preferred_foot")]
        public string PreferredFoot { get; set; } = string.Empty;

        [JsonPropertyName("age")]
        public int Age { get; set; }

        [JsonPropertyName("appearances")]
        public int Appearances { get; set; }

        [JsonPropertyName("subs")]
        public int Subs { get; set; }

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

        [JsonPropertyName("key_passes")]
        public int KeyPasses { get; set; }

        [JsonPropertyName("passes_completed")]
        public int PassesCompleted { get; set; }

        [JsonPropertyName("market_value_min")]
        public long MarketValueMin { get; set; }

        [JsonPropertyName("market_value_max")]
        public long MarketValueMax { get; set; }

        [JsonPropertyName("dribbles_made")]
        public int DribblesMade { get; set; }

        [JsonPropertyName("goals_per_90")]
        public decimal GoalsPer90 { get; set; }

        [JsonPropertyName("shots_on_target_per_90")]
        public decimal ShotsOnTargetPer90 { get; set; }

        [JsonPropertyName("assists_per_90")]
        public decimal AssistsPer90 { get; set; }

        [JsonPropertyName("key_passes_per_90")]
        public decimal KeyPassesPer90 { get; set; }

        [JsonPropertyName("passes_completed_per_90")]
        public decimal PassesCompletedPer90 { get; set; }

        [JsonPropertyName("conversion_rate")]
        public decimal ConversionRate { get; set; }

        [JsonPropertyName("shot_accuracy")]
        public decimal ShotAccuracy { get; set; }

        [JsonPropertyName("goal_involvement_per_90")]
        public decimal GoalInvolvementPer90 { get; set; }
        
        
        public StrikerRating? Rating { get; set; }
    }
}
