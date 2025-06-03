namespace Infrastructure.Persistance.Repositories;

public class StrikerRepository : IStrikerRepository
{
    private readonly AppDbContext _appDbContext;
    
    public StrikerRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }
    
    public async Task<StrikerEntity> Create(StrikerEntity strikerEntity)
    {
        await _appDbContext.Strikers.AddAsync(strikerEntity);
        await _appDbContext.SaveChangesAsync();
        
        return strikerEntity;
    }
}