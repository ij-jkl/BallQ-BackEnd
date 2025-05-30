namespace Domain.Interfaces;

public interface IDataLoadRepository
{
    Task<bool> ExecuteStrikerInsertScriptAsync();
}