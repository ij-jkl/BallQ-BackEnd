namespace Application.Validation.Strikers;

public class CreateStrikerCommandValidator : AbstractValidator<CreateStrikerCommand>
{
    public CreateStrikerCommandValidator()
    {
        RuleFor(x => x.Striker)
            .NotNull()
            .WithMessage("Striker object must be provided.");

        When(x => x.Striker != null, () =>
        {
            RuleFor(x => x.Striker.Name)
                .NotEmpty().WithMessage("Name is required.")
                .Must(IsNotPlaceholder).WithMessage("Name must be a valid, non-placeholder value.");

            RuleFor(x => x.Striker.Position)
                .NotEmpty().WithMessage("Position is required.")
                .Must(IsNotPlaceholder).WithMessage("Position must be a valid, non-placeholder value.");

            RuleFor(x => x.Striker.Club)
                .NotEmpty().WithMessage("Club is required.")
                .Must(IsNotPlaceholder).WithMessage("Club must be a valid, non-placeholder value.");

            RuleFor(x => x.Striker.Nationality)
                .NotEmpty().WithMessage("Nationality is required.")
                .Must(IsNotPlaceholder).WithMessage("Nationality must be a valid, non-placeholder value.");

            RuleFor(x => x.Striker.PreferredFoot)
                .NotEmpty().WithMessage("Preferred foot must be specified.")
                .Must(IsNotPlaceholder).WithMessage("Preferred foot must be a valid value.");

            RuleFor(x => x.Striker.Age)
                .InclusiveBetween(14, 55)
                .WithMessage("Age must be between 14 and 55.");

            RuleFor(x => x.Striker.HeightCm)
                .InclusiveBetween(140, 220)
                .WithMessage("Height should be between 140 cm and 220 cm.");

            RuleFor(x => x.Striker.WeightKg)
                .InclusiveBetween(50, 130)
                .WithMessage("Weight should be between 50 kg and 130 kg.");

            RuleFor(x => x.Striker.MinutesPlayed)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Minutes played cannot be negative.");

            RuleFor(x => x.Striker.Goals)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Goals cannot be negative.");

            RuleFor(x => x.Striker.Shots)
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.Striker.ShotsOnTarget)
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.Striker.Assists)
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.Striker.KeyPasses)
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.Striker.PassesCompleted)
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.Striker.MarketValueMin)
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.Striker.MarketValueMax)
                .GreaterThanOrEqualTo(x => x.Striker.MarketValueMin)
                .WithMessage("MarketValueMax must be greater than or equal to MarketValueMin.");
        });
    }

    private bool IsNotPlaceholder(string? input)
    {
        return !string.Equals(input?.Trim(), "string", StringComparison.OrdinalIgnoreCase);
    }
}
