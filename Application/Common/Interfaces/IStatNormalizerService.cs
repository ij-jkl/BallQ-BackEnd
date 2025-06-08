namespace Application.Common.Interfaces;

public interface IStatNormalizerService
{
    double NormalizeMinMax(double value, double min, double max);
    double NormalizePercentile(int index, int total);
}