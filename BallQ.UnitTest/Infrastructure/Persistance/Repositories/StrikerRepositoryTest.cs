namespace BallQ.UnitTest.Infrastructure.Persistance.Repositories;

public class StrikerRepositoryTests
{
    private readonly AppDbContext _context;
    private readonly StrikerRepository _repository;

    public StrikerRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new AppDbContext(options);
        _repository = new StrikerRepository(_context);
    }

    [Fact]
    public async Task Create_ShouldAddStriker()
    {
        var striker = new StrikerEntity { Name = "Messi", Position = "ST" };

        var result = await _repository.Create(striker);

        Assert.NotNull(result);
        Assert.Equal("Messi", result.Name);
        Assert.Equal("ST", result.Position);
        
        Assert.Single(await _context.Strikers.ToListAsync());
    }

    [Fact]
    public async Task GetById_ShouldReturnCorrectStriker()
    {
        var striker = new StrikerEntity { Name = "Ronaldo", Position = "ST" };
        
        await _context.Strikers.AddAsync(striker);
        await _context.SaveChangesAsync();

        var result = await _repository.GetById(striker.Id);

        Assert.NotNull(result);
        Assert.Equal(striker.Id, result.Id);
        Assert.Equal("Ronaldo", result.Name);
    }

    [Fact]
    public async Task Update_ShouldModifyStriker()
    {
        var striker = new StrikerEntity { Name = "Mbappe", Position = "ST" };
        
        await _context.Strikers.AddAsync(striker);
        await _context.SaveChangesAsync();

        striker.Name = "Mbappe Updated";
        striker.Position = "CF"; 
        
        var updated = await _repository.Update(striker);
        
        Assert.Equal("Mbappe Updated", updated.Name);
        Assert.Equal("CF", updated.Position);
    }
}
