namespace Application.Common.Interfaces;

public interface IScoreCalculatorService<T>
{
    double CalculateGoalScore(T player);
    double CalculatePassingScore(T player, List<T> allPlayers);
    double CalculateShootingScore(T player, List<T> allPlayers); 
    double CalculateInvolvementScore(T player);
    double CalculateFinalScore(double goalScore, double shootingScore, double passingScore, double involvementScore, double consistencyBonus);
}