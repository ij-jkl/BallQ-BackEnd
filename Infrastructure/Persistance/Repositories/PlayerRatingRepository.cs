namespace Infrastructure.Persistance.Repositories;

public class PlayerRatingRepository : IPlayerRatingRepository
{
    private readonly AppDbContext _appDbContext;

    public PlayerRatingRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task SaveAllStrikers(IEnumerable<RatingEntity> ratings)
    {
        _appDbContext.Ratings.AddRange(ratings);
        
        await _appDbContext.SaveChangesAsync();
    }
}