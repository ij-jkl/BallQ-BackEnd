namespace Infrastructure.Services;

public class StatNormalizerService : IStatNormalizerService
{
    public double NormalizeMinMax(double value, double min, double max)
    {
        if (max - min == 0)
            return 0;

        var normalized = (value - min) / (max - min);
        return Math.Clamp(normalized * 10, 0, 10);
    }

    public double NormalizePercentile(int index, int total)
    {
        if (total <= 1)
            return 0;

        return Math.Round(10.0 * index / (total - 1), 2);
    }
}