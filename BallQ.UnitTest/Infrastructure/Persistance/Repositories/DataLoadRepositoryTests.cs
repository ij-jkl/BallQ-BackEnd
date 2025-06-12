namespace BallQ.UnitTest.Infrastructure.Persistance.Repositories;

public class DataLoadRepositoryTests
{
    [Fact]
    public async Task ExecuteStrikerInsertScriptAsync_ShouldThrow_WhenConnectionStringMissing()
    {
        // Arrange
        // Backup .env file and create an empty one to see it fail
        var current = new DirectoryInfo(AppContext.BaseDirectory);
        string? envPath = null;

        while (current != null)
        {
            var candidate = Path.Combine(current.FullName, ".env");
            if (File.Exists(candidate) || Directory.Exists(Path.GetDirectoryName(candidate)))
            {
                envPath = candidate;
                break;
            }
            current = current.Parent;
        }

        if (envPath == null)
            throw new InvalidOperationException("No .env directory found for test setup");

        var backupPath = envPath + ".bak_test";
        if (File.Exists(envPath)) File.Move(envPath, backupPath);
        
        File.WriteAllText(envPath, ""); // Empty .env to simulate missing connection string

        try
        {
            Environment.SetEnvironmentVariable("MYSQL_CONNECTION_STRING", null, EnvironmentVariableTarget.Process);

            var scriptPath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "..", "db_scripts", "processed_strikers.sql"));
            Directory.CreateDirectory(Path.GetDirectoryName(scriptPath)!);
            
            File.WriteAllText(scriptPath, "-- dummy SQL");

            var repository = new DataLoadRepository();

            // Act & Assert
            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                repository.ExecuteStrikerInsertScriptAsync());

            Assert.Equal("MYSQL_CONNECTION_STRING is missing", ex.Message);

            File.Delete(scriptPath);
        }
        finally
        {
            // Restore .env
            if (File.Exists(backupPath)) File.Move(backupPath, envPath);
            else if (File.Exists(envPath)) File.Delete(envPath); // clean empty file
        }
    }



    [Fact]
    public async Task ExecuteStrikerInsertScriptAsync_ShouldThrow_WhenScriptFileDoesNotExist()
    {
        // Arrange
        Environment.SetEnvironmentVariable("MYSQL_CONNECTION_STRING", "Server=localhost;Database=test;Uid=root;Pwd=root;", EnvironmentVariableTarget.Process);
        var repository = new DataLoadRepository();

        var scriptPath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "..", "db_scripts", "processed_strikers.sql"));

        var renamed = false;
        var tempPath = scriptPath + ".bak";

        if (File.Exists(scriptPath))
        {
            File.Move(scriptPath, tempPath);
            renamed = true;
        }

        try
        {
            // Act & Assert
            await Assert.ThrowsAsync<FileNotFoundException>(() =>
                repository.ExecuteStrikerInsertScriptAsync());
        }
        finally
        {
            if (renamed)
            {
                File.Move(tempPath, scriptPath); // Restore
            }
        }
    }
}
