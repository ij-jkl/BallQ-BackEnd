namespace Application.Common.Exceptions;

public class UnsupportedPlayerTypeException : Exception
{
    public UnsupportedPlayerTypeException(Type playerType, Type ratingType)
        : base($"Rating generation is not supported for player type '{playerType.Name}' and rating type '{ratingType.Name}'.")
    {
    }
}
