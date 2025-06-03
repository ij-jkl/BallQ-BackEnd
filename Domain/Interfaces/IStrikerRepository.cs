namespace Domain.Interfaces;

public interface IStrikerRepository
{
    Task<StrikerEntity> Create(StrikerEntity strikerEntity);
}