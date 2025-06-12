namespace BallQ.UnitTest.Infrastructure.Services;

public class StatNormalizerServiceTests
{
    private readonly StatNormalizerService _service = new();

    [Theory]
    [InlineData(50, 0, 100, 5)]       // Midpoint
    [InlineData(100, 0, 100, 10)]     // Max
    [InlineData(0, 0, 100, 0)]        // Min
    [InlineData(150, 0, 100, 10)]     // Above max, should clamp to 10
    [InlineData(-50, 0, 100, 0)]      // Below min, should clamp to 0
    
    public void NormalizeMinMax_ReturnsExpectedResult(double value, double min, double max, double expected)
    {
        var result = _service.NormalizeMinMax(value, min, max);
        Assert.Equal(expected, result, precision: 2);
    }

    [Fact]
    public void NormalizeMinMax_ReturnsZero_WhenMinEqualsMax()
    {
        var result = _service.NormalizeMinMax(50, 100, 100);
        Assert.Equal(0, result);
    }

    [Theory]
    [InlineData(0, 5, 0.00)]       // First player
    [InlineData(1, 5, 2.50)]
    [InlineData(2, 5, 5.00)]
    [InlineData(3, 5, 7.50)]
    [InlineData(4, 5, 10.00)]      // Last player
    
    public void NormalizePercentile_ReturnsExpectedValue(int index, int total, double expected)
    {
        var result = _service.NormalizePercentile(index, total);
        Assert.Equal(expected, result, precision: 2);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    
    public void NormalizePercentile_ReturnsZero_WhenTotalIsLessThanOrEqualTo1(int total)
    {
        var result = _service.NormalizePercentile(0, total);
        Assert.Equal(0, result);
    }
}