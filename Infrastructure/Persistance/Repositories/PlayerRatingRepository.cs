public class PlayerRatingRepository : IPlayerRatingRepository
{
    private readonly AppDbContext _context;

    public PlayerRatingRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<RatingEntity>> GetAllRatings()
    {
        return await _context.Ratings.ToListAsync();
    }

    public async Task SaveAllStrikers(IEnumerable<RatingEntity> ratings)
    {
        foreach (var rating in ratings)
        {
            var existing = await _context.Ratings
                .FirstOrDefaultAsync(r => r.PlayerId == rating.PlayerId);

            if (existing != null)
            {
                existing.GoalScore = rating.GoalScore;
                existing.ShootingScore = rating.ShootingScore;
                existing.PassingScore = rating.PassingScore;
                existing.InvolvementScore = rating.InvolvementScore;
                existing.FinalScore = rating.FinalScore;
                existing.Position = rating.Position;
            }
            else
            {
                await _context.Ratings.AddAsync(rating);
            }
        }

        await _context.SaveChangesAsync();
    }
    
    public async Task<RatingEntity?> GetById(int id)
    {
        return await _context.Ratings
            .Include(r => r.Player) 
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    
    public IQueryable<RatingEntity> AsQueryable()
    {
        return _context.Ratings.AsQueryable()
            .Include(r => r.Player);
    }
    
    public async Task<List<RatingEntity>> GetTopRatingsAsync(int limit, bool ascending, CancellationToken cancellationToken)
    {
        var query = _context.Ratings
            .Include(r => r.Player)
            .AsQueryable();

        query = ascending
            ? query.OrderBy(r => r.FinalScore)
            : query.OrderByDescending(r => r.FinalScore);

        return await query
            .Take(limit)
            .ToListAsync(cancellationToken);
    }

}