namespace Application.Validation.RatePlayers;

public class CompareStrikersCommandValidator : AbstractValidator<CompareStrikersCommand>
{
    public CompareStrikersCommandValidator()
    {
        RuleFor(x => x.PlayerIds)
            .NotNull().WithMessage("PlayerIds list must not be null.")
            .NotEmpty().WithMessage("You must provide at least one player ID.")
            .Must(ids => ids.All(id => id > 0)).WithMessage("All player IDs must be greater than 0.")
            .Must(ids => ids.Distinct().Count() == ids.Count).WithMessage("Player IDs must be unique.")
            .Must(ids => ids.Count <= 10).WithMessage("You can compare a maximum of 10 players.");
    }
}