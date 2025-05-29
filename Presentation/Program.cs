// Load environment variables in Run Time
DotNetEnv.Env.Load();

var builder = WebApplication.CreateBuilder(args);

// Configure services
ConfigureServices(builder.Services);

var app = builder.Build();

// Configure middleware
ConfigureMiddleware(app);

app.Run();

// Command-line reference:
// dotnet ef migrations add InitialCreate -p Infrastructure -s Presentation
// dotnet ef database update -p Infrastructure -s Presentation

void ConfigureServices(IServiceCollection services)
{
    var connectionString = Environment.GetEnvironmentVariable("MYSQL_CONNECTION_STRING");

    if (string.IsNullOrWhiteSpace(connectionString))
        throw new InvalidOperationException("MYSQL_CONNECTION_STRING is not set in the environment variables (In Run Time).");

    services.AddDbContext<AppDbContext>(options =>
        options.UseMySql(
            connectionString,
            new MySqlServerVersion(new Version(8, 0, 36))
        ));

    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();
}

void ConfigureMiddleware(WebApplication app)
{
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
}