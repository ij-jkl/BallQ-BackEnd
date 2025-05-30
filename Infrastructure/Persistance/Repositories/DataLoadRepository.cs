namespace Infrastructure.Persistance.Repositories;

public class DataLoadRepository : IDataLoadRepository
{
    public async Task<bool> ExecuteStrikerInsertScriptAsync()
    {
        EnvLoader.LoadRootEnv();

        string connectionString = Environment.GetEnvironmentVariable("MYSQL_CONNECTION_STRING");

        Console.WriteLine("[DEBUG] MYSQL_CONNECTION_STRING = " + connectionString);

        if (string.IsNullOrEmpty(connectionString))
            throw new InvalidOperationException("MYSQL_CONNECTION_STRING is missing");
        
        string scriptPath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "..", "db_scripts", "insert_all_players_final_cleaned.sql"));

        Console.WriteLine("[DEBUG] Script path = " + scriptPath);

        if (!File.Exists(scriptPath))
            throw new FileNotFoundException("SQL script not found at: " + scriptPath);

        string sqlScript = await File.ReadAllTextAsync(scriptPath);

        using var connection = new MySql.Data.MySqlClient.MySqlConnection(connectionString);
        using var command = new MySql.Data.MySqlClient.MySqlCommand(sqlScript, connection);

        await connection.OpenAsync();
        await command.ExecuteNonQueryAsync();

        return true;
    }
}