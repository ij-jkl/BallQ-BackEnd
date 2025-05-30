namespace Infrastructure.Persistance.Repositories;

public class DataLoadRepository : IDataLoadRepository
{
    private readonly IConfiguration _configuration;

    public DataLoadRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<bool> ExecuteStrikerInsertScriptAsync()
    {
        string connectionString = _configuration.GetConnectionString("DefaultConnection");

        string scriptPath = Path.Combine(Directory.GetCurrentDirectory(), "db_scripts", "insert_all_strikers.sql");

        if (!File.Exists(scriptPath))
            return false;

        string sqlScript = await File.ReadAllTextAsync(scriptPath);

        using SqlConnection connection = new(connectionString);
        using SqlCommand command = new(sqlScript, connection);

        await connection.OpenAsync();
        await command.ExecuteNonQueryAsync();

        return true;
    }
}