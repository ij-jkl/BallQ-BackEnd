using DotNetEnv;

namespace Infrastructure.Utils;

public static class EnvLoader
{
    public static void LoadRootEnv()
    {
        var rootEnvPath = Path.Combine(AppContext.BaseDirectory, @"..\..\..\..\.env");
        Env.Load(Path.GetFullPath(rootEnvPath));
    }
}