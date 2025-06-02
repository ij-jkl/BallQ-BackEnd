public static class EnvLoader
{
    public static void LoadRootEnv()
    {
        var current = new DirectoryInfo(AppContext.BaseDirectory);

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