namespace Domain.Interfaces;

public interface IStrikerRepository
{
    Task<StrikerEntity> Create(StrikerEntity strikerEntity);
    Task<List<StrikerEntity>> GetAllStrikers();
    Task<StrikerEntity> GetById(int id);
    Task<StrikerEntity> Update(StrikerEntity strikerEntity);
    IQueryable<StrikerEntity> AsQueryable();
    Task AddRangeAsync(IEnumerable<StrikerEntity> strikers);
}