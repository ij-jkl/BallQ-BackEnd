namespace Domain.Interfaces;

public interface IPlayerRatingRepository
{
    Task<List<RatingEntity>> GetAllRatings(); 
    Task SaveAllStrikers(IEnumerable<RatingEntity> ratings); 
    Task<RatingEntity> GetById(int id);
    Task<List<RatingEntity>> GetTopRatingsAsync(int limit, bool ascending, CancellationToken cancellationToken);
    IQueryable<RatingEntity> AsQueryable();
    Task<List<RatingEntity>> GetMultiplePlayerByIds(List<int> ids);
    Task<RatingEntity?> GetRandomPlayerRatings();
}