namespace Domain.Interfaces;

public interface IPlayerRatingRepository
{
    Task SaveAllStrikers(IEnumerable<RatingEntity> ratings);
}