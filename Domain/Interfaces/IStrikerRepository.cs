namespace Domain.Interfaces;

public interface IStrikerRepository
{
    Task<StrikerEntity> Create(StrikerEntity strikerEntity);
    Task<StrikerEntity> GetById(int id);
    Task<StrikerEntity> Update(StrikerEntity strikerEntity);
    IQueryable<StrikerEntity> AsQueryable();
}