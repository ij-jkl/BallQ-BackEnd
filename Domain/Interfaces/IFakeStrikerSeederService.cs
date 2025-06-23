namespace Domain.Interfaces
{
    public interface IFakeStrikerSeederService
    {
        List<StrikerEntity> GenerateFakeStrikers(int quantity);
    }
}