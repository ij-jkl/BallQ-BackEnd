namespace Application.Common.Interfaces;

public interface IScoreCalculatorService<in T>
{
    double CalculateGoalScore(T player);
    double CalculateShootingScore(T player);
    double CalculatePassingScore(T player);
    double CalculateInvolvementScore(T player);
    double CalculateFinalScore(double goalScore,double shootingScore,double passingScore,double involvementScore,double consistencyBonus);
}