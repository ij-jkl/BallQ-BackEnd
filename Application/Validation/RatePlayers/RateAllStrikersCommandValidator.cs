namespace Application.Validation.RatePlayers;

public class RateAllStrikersCommandValidator : AbstractValidator<RateAllStrikersCommand>
{
    public RateAllStrikersCommandValidator()
    {
        RuleFor(_ => _)
            .NotNull()
            .WithMessage("RateAllStrikersCommand cannot be null.");
    }
}