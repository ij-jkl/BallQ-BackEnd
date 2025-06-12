namespace BallQ.UnitTest.Infrastructure.Persistance;

public class AppDbContextFactoryTests
{
    [Fact]
    public void CreateDbContext_ShouldThrow_WhenEnvFileIsMissing()
    {
        // Backup and remove .env files, to simulate missing environment variables
        var backups = new List<(string Original, string Backup)>();
        var current = new DirectoryInfo(AppContext.BaseDirectory);

        while (current != null)
        {
            var envPath = Path.Combine(current.FullName, ".env");
            var backupPath = envPath + ".bak_test";

            if (File.Exists(envPath))
            {
                File.Move(envPath, backupPath);
                backups.Add((envPath, backupPath));
            }

            current = current.Parent;
        } try
        {
            Environment.SetEnvironmentVariable("MYSQL_CONNECTION_STRING", null);

            var factory = new AppDbContextFactory();

            var ex = Assert.Throws<FileNotFoundException>(() =>
            {
                factory.CreateDbContext(Array.Empty<string>());
            });

            Assert.Contains(".env", ex.Message);
        }
        finally
        {
            // Restore .env files
            foreach (var (original, backup) in backups)
            {
                if (File.Exists(backup))
                    File.Move(backup, original);
            }
        }
    }

    
    [Fact]
    public void CreateDbContext_ShouldReturnContext_WhenConnectionStringIsSet()
    {
        // Arrange
        var fakeConnection = "server=localhost;user=root;password=root;database=balliq_test;";
        Environment.SetEnvironmentVariable("MYSQL_CONNECTION_STRING", fakeConnection);

        var factory = new AppDbContextFactory();

        // Act
        var context = factory.CreateDbContext(Array.Empty<string>());

        // Assert
        Assert.NotNull(context);
        Assert.IsType<AppDbContext>(context);
    }
}