namespace BallQ.UnitTest.Infrastructure.Persistance.Repositories;

public class PlayerRatingRepositoryTests
{
    private readonly AppDbContext _context;
    private readonly PlayerRatingRepository _repository;

    public PlayerRatingRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) 
            .Options;

        _context = new AppDbContext(options);
        _repository = new PlayerRatingRepository(_context);
    }

    [Fact]
    public async Task GetAllRatings_ReturnsEmptyList_WhenNoRatingsExist()
    {
        var result = await _repository.GetAllRatings();
        Assert.Empty(result);
    }

    [Fact]
    public async Task SaveAllStrikers_AddsNewRating_WhenNotExists()
    {
        var rating = new RatingEntity { PlayerId = 1, GoalScore = 8, Position = "ST" };

        await _repository.SaveAllStrikers(new List<RatingEntity> { rating });

        var stored = await _context.Ratings.FirstOrDefaultAsync();
        
        Assert.NotNull(stored);
        Assert.Equal(8, stored.GoalScore);
        Assert.Equal("ST", stored.Position);
    }

    [Fact]
    public async Task SaveAllStrikers_UpdatesExistingRating_WhenAlreadyExists()
    {
        var rating = new RatingEntity { PlayerId = 2, GoalScore = 5, Position = "ST" };
        
        await _context.Ratings.AddAsync(rating);
        await _context.SaveChangesAsync();

        var updated = new RatingEntity { PlayerId = 2, GoalScore = 9, Position = "ST" };
        await _repository.SaveAllStrikers(new List<RatingEntity> { updated });

        var result = await _context.Ratings.FirstOrDefaultAsync(r => r.PlayerId == 2);
        
        Assert.Equal(9, result!.GoalScore);
        Assert.Equal("ST", result.Position);
    }

    [Fact]
    public async Task GetById_ReturnsRating_WhenExists()
    {
        var striker = new StrikerEntity { Id = 3, Name = "Haaland", Position = "ST" };
        await _context.Strikers.AddAsync(striker);
        
        var rating = new RatingEntity { PlayerId = 3, GoalScore = 6, Position = "ST" };
        await _context.Ratings.AddAsync(rating);
        await _context.SaveChangesAsync(); 

        var result = await _repository.GetById(rating.Id); 

        Assert.NotNull(result);
        Assert.Equal(6, result!.GoalScore);
        Assert.Equal("ST", result.Position);
        Assert.NotNull(result.Player);
        Assert.Equal("Haaland", result.Player!.Name);
    }



    [Fact]
    public async Task GetTopRatingsAsync_ReturnsCorrectlyOrdered_WhenDescending()
    {
        await _context.Strikers.AddRangeAsync(
            new StrikerEntity { Id = 10 },
            new StrikerEntity { Id = 11 },
            new StrikerEntity { Id = 12 }
        );

        await _context.Ratings.AddRangeAsync(
            new RatingEntity { PlayerId = 10, FinalScore = 6, Position = "ST" },
            new RatingEntity { PlayerId = 11, FinalScore = 9, Position = "ST" },
            new RatingEntity { PlayerId = 12, FinalScore = 3, Position = "ST" }
        );
        await _context.SaveChangesAsync();

        var result = await _repository.GetTopRatingsAsync(2, ascending: false, CancellationToken.None);

        Assert.Equal(2, result.Count);
        Assert.Equal(9, result[0].FinalScore);
        Assert.Equal(6, result[1].FinalScore);
    }


    [Fact]
    public async Task GetMultiplePlayerByIds_ReturnsMatchingPlayers()
    {
        await _context.Strikers.AddRangeAsync(
            new StrikerEntity { Id = 20, Name = "A" },
            new StrikerEntity { Id = 21, Name = "B" }
        );

        await _context.Ratings.AddRangeAsync(
            new RatingEntity { PlayerId = 20, FinalScore = 7, Position = "ST" },
            new RatingEntity { PlayerId = 21, FinalScore = 8, Position = "ST" }
        );
        await _context.SaveChangesAsync();

        var result = await _repository.GetMultiplePlayerByIds(new List<int> { 20 });

        Assert.Single(result);
        Assert.Equal(20, result[0].PlayerId);
        Assert.Equal(7, result[0].FinalScore);
        Assert.DoesNotContain(result, r => r.PlayerId == 21);
    }


    [Fact]
    public async Task GetRandomPlayerRatings_ReturnsOneRandom_WhenRatingsExist()
    {
        await _context.Strikers.AddRangeAsync(
            new StrikerEntity { Id = 30, Name = "A" },
            new StrikerEntity { Id = 31, Name = "B" }
        );

        await _context.Ratings.AddRangeAsync(
            new RatingEntity { PlayerId = 30, GoalScore = 7, FinalScore = 9, Position = "ST" },
            new RatingEntity { PlayerId = 31, GoalScore = 6, FinalScore = 8, Position = "ST" }
        );
        await _context.SaveChangesAsync();

        var result = await _repository.GetRandomPlayerRatings();

        Assert.NotNull(result);
        Assert.Contains(result!.PlayerId, new[] { 30, 31 });
        Assert.NotEqual(0, result.GoalScore);     // Optional: more reliable than expecting exact value
        Assert.NotEqual(0, result.FinalScore);
        Assert.NotNull(result.Player);            // Validate Include
    }


    [Fact]
    public async Task GetRandomPlayerRatings_ReturnsNull_WhenNoRatings()
    {
        var result = await _repository.GetRandomPlayerRatings();
        
        Assert.Null(result);
    }
}
