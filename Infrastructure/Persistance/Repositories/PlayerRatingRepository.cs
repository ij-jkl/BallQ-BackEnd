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
}