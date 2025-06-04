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

    public async Task<StrikerEntity> GetById(int id)
    {
        return await _appDbContext.Strikers.FindAsync(id);
    }
    
    public async Task<StrikerEntity> Update(StrikerEntity strikerEntity)
    {
        
        if (_appDbContext.Entry(strikerEntity).State == EntityState.Detached)
        {
            _appDbContext.Strikers.Attach(strikerEntity);
        }
        
        await _appDbContext.SaveChangesAsync();
        return strikerEntity;
    }

    public IQueryable<StrikerEntity> AsQueryable()
    {
        return _appDbContext.Strikers.AsQueryable();
    }

}