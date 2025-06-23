namespace Infrastructure.Data.Seeding;

public class FakeStrikerSeederService : IFakeStrikerSeederService
{
    private static readonly string[] AttackerPositions =
    {
        "ST (C)", "ST (L)", "ST (R)", "RW", "LW"
    };

    public List<StrikerEntity> GenerateFakeStrikers(int quantity)
    {
        var faker = new Faker("en");
        var random = new Random();
        var strikers = new List<StrikerEntity>();

        for (int i = 0; i < quantity; i++)
        {
            // Randomly assign a performance tier to simulate skill levels
            int tier = GetRandomTier(random);
            int age = faker.Random.Int(18, 35);
            int appearances = random.Next(5, 38);

            /*
             Define the probability of the player starting based on tier:
             Higher tiers mean higher chances of starting.
            */
            double startProbability = tier switch
            {
                0 => faker.Random.Double(0.0, 0.4),
                1 => faker.Random.Double(0.3, 0.6),
                2 => faker.Random.Double(0.6, 0.85),
                3 => faker.Random.Double(0.85, 0.95),
                4 => faker.Random.Double(0.95, 1.0),
                _ => 0.5
            };

            int starts = (int)Math.Round(appearances * startProbability);
            int subs = appearances - starts;

            /*
             Simulate realistic total minutes played based on role in match:
             - Starters play 75-95 minutes.
             - Substitutes play 10-30 minutes.
            */
            int minutesFromStarts = starts * random.Next(75, 96); 
            int minutesFromSubs = subs * random.Next(10, 31);     
            
            int minutesPlayed = minutesFromStarts + minutesFromSubs;

            // Initialize key stat variables
            int goals = 0, assists = 0, shots = 0, shotsOnTarget = 0, keyPasses = 0, passesCompleted = 0, dribblesMade = 0;
            int marketValueMin = 0, marketValueMax = 0;

            string position = faker.PickRandom(AttackerPositions);

            /*
             Identify type of attacker to tweak stat tendencies:
             - Wingers: more dribbles.
             - Central strikers: more shots/passes.
            */
            
            bool isWingerLike = position.Contains("RW") || position.Contains("LW");
            bool isCentralStriker = position.Contains("ST") && position.Contains("C") && !isWingerLike;

            // Apply shooting boost to top-tier central strikers
            int shotBoost = (tier >= 2 && isCentralStriker) ? (int)Math.Ceiling((tier == 4 ? 1.4 : 1.2) * 5) : 0;

            /*
             Stat generation based on tier and player type:
             - Higher tiers yield better stats and market value.
             - Each tier has different ranges for assists, shots, passes, etc.
            */
            switch (tier)
            {
                case 0:
                    assists = random.Next(0, 1);
                    shots = random.Next(2, 6);
                    passesCompleted = isWingerLike ? random.Next(30, 100) : random.Next(10, 80);
                    dribblesMade = isWingerLike ? random.Next(0, 5) : 0;
                    marketValueMin = random.Next(50_000, 200_000);
                    marketValueMax = random.Next(200_000, 400_000);
                    break;
                case 1:
                    assists = random.Next(0, 2);
                    shots = random.Next(6, 15);
                    passesCompleted = isWingerLike ? random.Next(50, 180) : random.Next(30, 140);
                    dribblesMade = isWingerLike ? random.Next(5, 15) : random.Next(0, 5);
                    marketValueMin = random.Next(400_000, 1_000_000);
                    marketValueMax = random.Next(1_000_000, 2_000_000);
                    break;
                case 2:
                    assists = random.Next(2, 5);
                    shots = Math.Min(random.Next(14 + shotBoost, 32 + shotBoost), 70);
                    keyPasses = random.Next(4, 10);
                    dribblesMade = isWingerLike ? random.Next(20, 50) : random.Next(5, 20);
                    passesCompleted = isCentralStriker ? random.Next(80, 250) : random.Next(120, 400);
                    marketValueMin = random.Next(2_000_000, 8_000_000);
                    marketValueMax = random.Next(8_000_000, 18_000_000);
                    break;
                case 3:
                    assists = random.Next(4, 7);
                    shots = Math.Min(random.Next(28 + shotBoost, 50 + shotBoost), 80);
                    keyPasses = random.Next(8, 15);
                    dribblesMade = isWingerLike ? random.Next(35, 65) : random.Next(15, 45);
                    passesCompleted = isCentralStriker ? random.Next(120, 300) : random.Next(200, 600);
                    marketValueMin = random.Next(18_000_000, 35_000_000);
                    marketValueMax = random.Next(35_000_000, 55_000_000);
                    break;
                case 4:
                    assists = random.Next(6, 9);
                    shots = Math.Min(random.Next(40 + shotBoost, 60 + shotBoost), 80);
                    keyPasses = random.Next(12, 18);
                    dribblesMade = isWingerLike ? random.Next(60, 80) : random.Next(25, 60);
                    passesCompleted = isCentralStriker ? random.Next(150, 400) : random.Next(350, 800);
                    marketValueMin = random.Next(55_000_000, 85_000_000);
                    marketValueMax = random.Next(85_000_000, 120_000_000);
                    break;
            }

            // Determine shots on target using based on tier for realistic accuracy
            double maxAccuracy = tier switch
            {
                <= 1 => faker.Random.Double(0.35, 0.60),
                2 => faker.Random.Double(0.60, 0.75),
                _ => faker.Random.Double(0.75, 0.90)
            };
            shotsOnTarget = (int)Math.Round(shots * maxAccuracy);
            shotsOnTarget = Math.Min(shotsOnTarget, shots);

            // Determine goals using realistic conversion rates by tier
            double maxConversion = tier switch
            {
                <= 1 => faker.Random.Double(0.08, 0.18),
                2 => faker.Random.Double(0.15, 0.25),
                _ => faker.Random.Double(0.25, 0.33)
            };
            goals = (int)Math.Round(shots * maxConversion);
            goals = Math.Min(goals, shots);

            // Ensure assists are never greater than key passes
            keyPasses = Math.Max(keyPasses, assists);

            // If a striker played over 2500 minutes but had no goals/assists, give minimal contribution 
            // to simulate a player who is still involved but not scoring
            if (minutesPlayed > 2500 && goals + assists == 0)
            {
                goals = random.Next(1, 3);
                assists = random.Next(0, 2);
            }

            // Calculate advanced per-90 and percentage-based stats
            decimal conversionRate = shots > 0 ? Math.Round((decimal)goals / shots * 100, 2) : 0;
            decimal shotAccuracy = shots > 0 ? Math.Round((decimal)shotsOnTarget / shots * 100, 2) : 0;

            decimal goalsPer90 = minutesPlayed > 0 ? Math.Round((decimal)goals / minutesPlayed * 90, 10) : 0;
            decimal assistsPer90 = minutesPlayed > 0 ? Math.Round((decimal)assists / minutesPlayed * 90, 10) : 0;
            decimal shotsOnTargetPer90 = minutesPlayed > 0 ? Math.Round((decimal)shotsOnTarget / minutesPlayed * 90, 10) : 0;
            decimal keyPassesPer90 = minutesPlayed > 0 ? Math.Round((decimal)keyPasses / minutesPlayed * 90, 10) : 0;
            decimal passesCompletedPer90 = minutesPlayed > 0 ? Math.Round((decimal)passesCompleted / minutesPlayed * 90, 10) : 0;
            decimal goalInvolvementPer90 = minutesPlayed > 0 ? Math.Round((decimal)(goals + assists) / minutesPlayed * 90, 10) : 0;

            // Scale market value by age impact (peak around age 25)
            decimal ageFactor = 1 - ((decimal)(age - 25) / 60);
            ageFactor = Math.Clamp(ageFactor, 0.75M, 1.1M);
            
            marketValueMin = (int)(marketValueMin * ageFactor);
            marketValueMax = (int)(marketValueMax * ageFactor);

            // Estimate expected market value based on performance and age
            decimal expectedMarketValue = CalculateExpectedMarketValue(
                age, goalsPer90, assistsPer90, shotAccuracy,
                passesCompletedPer90, dribblesMade
            );

            // Adjust value based on how far actual value is from expected
            decimal marketValueAdjustmentFactor = CalculateMarketValueAdjustmentFactor(
                expectedMarketValue, marketValueMin, marketValueMax, age, tier
            );

            marketValueMin = (int)(marketValueMin * marketValueAdjustmentFactor);
            marketValueMax = (int)(marketValueMax * marketValueAdjustmentFactor);

            // Final striker object creation
            strikers.Add(new StrikerEntity
            {
                Name = faker.Name.FirstName(Bogus.DataSets.Name.Gender.Male) + " " + faker.Name.LastName(),
                Position = position,
                Club = faker.Company.CompanyName(),
                Nationality = faker.Address.CountryCode(),
                HeightCm = Math.Round(faker.Random.Double(165, 195), 2),
                WeightKg = Math.Round(faker.Random.Double(60, 90), 2),
                PreferredFoot = faker.Random.Bool() ? "Right" : "Left",
                Age = age,
                Appearances = appearances,
                Subs = subs,
                Starts = starts,
                MinutesPlayed = minutesPlayed,
                Goals = goals,
                Shots = shots,
                ShotsOnTarget = shotsOnTarget,
                Assists = assists,
                KeyPasses = keyPasses,
                PassesCompleted = passesCompleted,
                MarketValueMin = marketValueMin,
                MarketValueMax = marketValueMax,
                DribblesMade = dribblesMade,
                GoalsPer90 = goalsPer90,
                ShotsOnTargetPer90 = shotsOnTargetPer90,
                AssistsPer90 = assistsPer90,
                KeyPassesPer90 = keyPassesPer90,
                PassesCompletedPer90 = passesCompletedPer90,
                ConversionRate = conversionRate,
                ShotAccuracy = shotAccuracy,
                GoalInvolvementPer90 = goalInvolvementPer90
            });
        }

        return strikers;
    }

    // Determines a random tier using weighted probabilities
    private int GetRandomTier(Random random)
    {
        double roll = random.NextDouble();

        if (roll < 0.45) return 0;          // Low-tier (Joselu,Timo Werner)
        else if (roll < 0.932) return 1;    // Mid-tier (Alvaro Morata , Duvan Zapata)
        else if (roll < 0.992) return 2;    // Top-tier (Darwin Nunez, Victor Osimhen)
        else if (roll < 0.999) return 3;    // Elite players (Julian Alvarez, Lautaro Martinez)
        else return 4;                      // World class (Messi-Neymar)
    }

    // Calculates a projected market value based on weighted performance metrics
    private decimal CalculateExpectedMarketValue(
        int age,
        decimal goalsPer90,
        decimal assistsPer90,
        decimal shotAccuracy,
        decimal passesCompletedPer90,
        int dribblesMade)
    {
        decimal ageWeight = 0.15M;
        decimal goalsWeight = 0.30M;
        decimal assistsWeight = 0.20M;
        decimal shotAccuracyWeight = 0.10M;
        decimal passesWeight = 0.10M;
        decimal dribblesWeight = 0.15M;

        decimal normalizedAge = 1 - ((decimal)(age - 18) / (35 - 18));

        decimal expectedValue =
            (normalizedAge * ageWeight) +
            (goalsPer90 * goalsWeight) +
            (assistsPer90 * assistsWeight) +
            (shotAccuracy / 100 * shotAccuracyWeight) +
            (passesCompletedPer90 / 100 * passesWeight) +
            (dribblesMade / 100 * dribblesWeight);

        return expectedValue * 1_000_000;
    }

    // Compares expected vs actual market value and returns a scaling factor
    private decimal CalculateMarketValueAdjustmentFactor(
        decimal expectedMarketValue,
        int marketValueMin,
        int marketValueMax,
        int age,
        int tier)
    {
        decimal actualMarketValue = (marketValueMin + marketValueMax) / 2;
        decimal difference = expectedMarketValue - actualMarketValue;
        // Base value to calculate
        decimal adjustmentFactor = 1;

        decimal outlierThreshold = (tier >= 3) ? 0.3M : 0.5M;
        decimal maxAdjustment = (tier >= 3) ? 0.5M : 0.3M;

        if (difference > expectedMarketValue * outlierThreshold)
        {
            adjustmentFactor = 1 + Math.Min(maxAdjustment, difference / expectedMarketValue);
        }
        else if (difference < -expectedMarketValue * outlierThreshold)
        {
            adjustmentFactor = 1 - Math.Min(maxAdjustment, Math.Abs(difference) / expectedMarketValue);
        }

        return adjustmentFactor;
    }
}