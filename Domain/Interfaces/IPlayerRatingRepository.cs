namespace Domain.Interfaces;

public interface IPlayerRatingRepository
{
    Task<List<RatingEntity>> GetAllRatings(); 
    Task SaveAllStrikers(IEnumerable<RatingEntity> ratings); 
}