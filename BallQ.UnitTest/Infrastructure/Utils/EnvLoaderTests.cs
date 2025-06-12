namespace BallQ.UnitTest.Infrastructure.Utils;

public class EnvLoaderTests
{
    public class TestableEnvLoader
    {
        private readonly string _startPath;

        public TestableEnvLoader(string startPath)
        {
            _startPath = startPath;
        }

        public void LoadRootEnv()
        {
            var current = new DirectoryInfo(_startPath);

            while (current != null && !File.Exists(Path.Combine(current.FullName, ".env")))
            {
                current = current.Parent;
            }

            if (current == null)
                throw new FileNotFoundException("'.env' file not found in any parent directory.");

            var envPath = Path.Combine(current.FullName, ".env");
            DotNetEnv.Env.Load(envPath);
        }
    }

    [Fact]
    public void LoadRootEnv_ShouldThrow_WhenEnvFileIsMissing()
    {
        // Temp directory for testing without .env file
        var isolatedRoot = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(isolatedRoot);

        // testable class instance
        var envLoader = new TestableEnvLoader(isolatedRoot);

        // Act & Assert
        var ex = Assert.Throws<FileNotFoundException>(() => envLoader.LoadRootEnv());
        Assert.Contains(".env", ex.Message);

        Directory.Delete(isolatedRoot, true);
    }

    [Fact]
    public void LoadRootEnv_ShouldLoadEnvFile_WhenExists()
    {
        // Arrange
        var tempRoot = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(tempRoot);

        var envFilePath = Path.Combine(tempRoot, ".env");
        File.WriteAllText(envFilePath, "MY_TEST_KEY=my_test_value");

        var envLoader = new TestableEnvLoader(tempRoot);

        try
        {
            // Act
            envLoader.LoadRootEnv();

            // Assert
            var value = Environment.GetEnvironmentVariable("MY_TEST_KEY");
            Assert.Equal("my_test_value", value);
        }
        finally
        {
            Directory.Delete(tempRoot, true);
        }
    }
}