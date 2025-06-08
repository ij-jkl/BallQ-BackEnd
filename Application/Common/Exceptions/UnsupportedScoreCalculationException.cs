namespace Application.Common.Exceptions;

public class UnsupportedScoreCalculationException : Exception
{
    public UnsupportedScoreCalculationException(string scoreType, Type playerType)
        : base($"Score calculation for '{scoreType}' is not implemented for player type '{playerType.Name}'.")
    {
    }
}