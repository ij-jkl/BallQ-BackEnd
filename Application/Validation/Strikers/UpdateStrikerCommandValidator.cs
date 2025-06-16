namespace Application.Validation.Strikers;

public class UpdateStrikerCommandValidator : AbstractValidator<UpdateStrikerCommand>
{
    public UpdateStrikerCommandValidator()
    {
        RuleFor(x => x.Striker)
            .NotNull().WithMessage("Striker object must be provided.");

        When(x => x.Striker != null, () =>
        {
            RuleFor(x => x.Striker.Id)
                .GreaterThan(0).WithMessage("Striker ID must be greater than 0.");

            When(x => x.Striker.Name != null, () =>
            {
                RuleFor(x => x.Striker.Name)
                    .Must(IsNotPlaceholder).WithMessage("Name must be a valid, non-placeholder value.");
            });

            When(x => x.Striker.Position != null, () =>
            {
                RuleFor(x => x.Striker.Position)
                    .Must(IsNotPlaceholder).WithMessage("Position must be a valid, non-placeholder value.");
            });

            When(x => x.Striker.Club != null, () =>
            {
                RuleFor(x => x.Striker.Club)
                    .Must(IsNotPlaceholder).WithMessage("Club must be a valid, non-placeholder value.");
            });

            When(x => x.Striker.Nationality != null, () =>
            {
                RuleFor(x => x.Striker.Nationality)
                    .Must(IsNotPlaceholder).WithMessage("Nationality must be a valid, non-placeholder value.");
            });

            When(x => x.Striker.PreferredFoot != null, () =>
            {
                RuleFor(x => x.Striker.PreferredFoot)
                    .Must(IsNotPlaceholder).WithMessage("Preferred foot must be a valid value.");
            });

            When(x => x.Striker.Age.HasValue, () =>
            {
                RuleFor(x => x.Striker.Age!.Value)
                    .InclusiveBetween(14, 55).WithMessage("Age must be between 14 and 55.");
            });

            When(x => x.Striker.HeightCm.HasValue, () =>
            {
                RuleFor(x => x.Striker.HeightCm!.Value)
                    .InclusiveBetween(140, 220).WithMessage("Height should be between 140 cm and 220 cm.");
            });

            When(x => x.Striker.WeightKg.HasValue, () =>
            {
                RuleFor(x => x.Striker.WeightKg!.Value)
                    .InclusiveBetween(50, 130).WithMessage("Weight should be between 50 kg and 130 kg.");
            });

            When(x => x.Striker.MinutesPlayed.HasValue, () =>
            {
                RuleFor(x => x.Striker.MinutesPlayed!.Value)
                    .GreaterThanOrEqualTo(0).WithMessage("Minutes played cannot be negative.");
            });

            When(x => x.Striker.Goals.HasValue, () =>
            {
                RuleFor(x => x.Striker.Goals!.Value)
                    .GreaterThanOrEqualTo(0).WithMessage("Goals cannot be negative.");
            });

            When(x => x.Striker.MarketValueMin.HasValue, () =>
            {
                RuleFor(x => x.Striker.MarketValueMin!.Value)
                    .GreaterThanOrEqualTo(0);
            });

            When(x => x.Striker.MarketValueMax.HasValue, () =>
            {
                RuleFor(x => x.Striker.MarketValueMax!.Value)
                    .GreaterThanOrEqualTo(x => x.Striker.MarketValueMin ?? 0)
                    .WithMessage("MarketValueMax must be greater than or equal to MarketValueMin.");
            });
        });
    }

    private bool IsNotPlaceholder(string? input)
    {
        return !string.Equals(input?.Trim(), "string", StringComparison.OrdinalIgnoreCase);
    }
}
