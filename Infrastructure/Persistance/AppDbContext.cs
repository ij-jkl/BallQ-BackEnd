namespace Infrastructure.Persistance;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<StrikerEntity> Strikers { get; set; }
    public DbSet<StrikerRating> StrikerRatings { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<StrikerEntity>();

        entity.ToTable("strikers");
        
        entity.HasKey(p => p.Id);
        entity.Property(p => p.Id).HasColumnName("Id").ValueGeneratedOnAdd();
        
        entity.Property(p => p.Name).HasColumnName("Name");
        entity.Property(p => p.Position).HasColumnName("Position");
        entity.Property(p => p.Club).HasColumnName("Club");
        entity.Property(p => p.Nationality).HasColumnName("Nationality");
        entity.Property(p => p.HeightCm).HasColumnName("HeightCm");
        entity.Property(p => p.WeightKg).HasColumnName("WeightKg");
        entity.Property(p => p.PreferredFoot).HasColumnName("PreferredFoot");
        entity.Property(p => p.Age).HasColumnName("Age");
        entity.Property(p => p.Appearances).HasColumnName("Appearances");
        entity.Property(p => p.Subs).HasColumnName("Subs");
        entity.Property(p => p.Starts).HasColumnName("Starts");
        entity.Property(p => p.MinutesPlayed).HasColumnName("MinutesPlayed");
        entity.Property(p => p.Goals).HasColumnName("Goals");
        entity.Property(p => p.Shots).HasColumnName("Shots");
        entity.Property(p => p.ShotsOnTarget).HasColumnName("ShotsOnTarget");
        entity.Property(p => p.Assists).HasColumnName("Assists");
        entity.Property(p => p.KeyPasses).HasColumnName("KeyPasses");
        entity.Property(p => p.PassesCompleted).HasColumnName("PassesCompleted");
        entity.Property(p => p.MarketValueMin).HasColumnName("MarketValueMin");
        entity.Property(p => p.MarketValueMax).HasColumnName("MarketValueMax");
        entity.Property(p => p.DribblesMade).HasColumnName("DribblesMade");
        entity.Property(p => p.GoalsPer90).HasColumnName("GoalsPer90");
        entity.Property(p => p.ShotsOnTargetPer90).HasColumnName("ShotsOnTargetPer90");
        entity.Property(p => p.AssistsPer90).HasColumnName("AssistsPer90");
        entity.Property(p => p.KeyPassesPer90).HasColumnName("KeyPassesPer90");
        entity.Property(p => p.PassesCompletedPer90).HasColumnName("PassesCompletedPer90");
        entity.Property(p => p.ConversionRate).HasColumnName("ConversionRate");
        entity.Property(p => p.ShotAccuracy).HasColumnName("ShotAccuracy");
        entity.Property(p => p.GoalInvolvementPer90).HasColumnName("GoalInvolvementPer90");
        
        
        var ratingEntity = modelBuilder.Entity<StrikerRating>();

        ratingEntity.ToTable("striker_ratings");

        ratingEntity.HasKey(r => r.Id);
        ratingEntity.Property(r => r.Id).HasColumnName("Id").ValueGeneratedOnAdd();

        ratingEntity.Property(r => r.GoalScore).HasColumnName("GoalScore");
        ratingEntity.Property(r => r.PassingScore).HasColumnName("PassingScore");
        ratingEntity.Property(r => r.ShootingScore).HasColumnName("ShootingScore");
        ratingEntity.Property(r => r.InvolvementScore).HasColumnName("InvolvementScore");
        ratingEntity.Property(r => r.FinalScore).HasColumnName("FinalScore");

        ratingEntity.Property(r => r.StrikerId).HasColumnName("StrikerId");

        ratingEntity.HasOne(r => r.Striker)
            .WithOne(s => s.Rating)
            .HasForeignKey<StrikerRating>(r => r.StrikerId)
            .HasConstraintName("FK_StrikerRating_Striker");
    }
}
// To create the initial migration and update the database, run the following commands in the terminal, after we are going to run everything just by docker compose:
// dotnet ef migrations add InitialCreate -p Infrastructure -s Presentation
// dotnet ef database update -p Infrastructure -s Presentation