namespace Application.Common.Interfaces;

public interface IPlayerRatingService<TPlayer, TRating>
{
    Task<TRating> GenerateStrikerRating(TPlayer player, List<TPlayer> allPlayers);
}