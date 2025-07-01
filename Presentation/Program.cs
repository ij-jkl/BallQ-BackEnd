// Load MYSQL_CONNECTION_STRING before building the application
EnvLoader.LoadRootEnv();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddInfrastructure();

// Register application services
builder.Services.AddScoped<IDataLoadRepository, DataLoadRepository>();
builder.Services.AddScoped<IStrikerRepository, StrikerRepository>();
builder.Services.AddScoped<IPaginationService, PaginationService>();
builder.Services.AddScoped<IPlayerRatingRepository, PlayerRatingRepository>();
builder.Services.AddScoped<IScoreCalculatorService<StrikerEntity>, PlayerScoreCalculator<StrikerEntity>>();
builder.Services.AddScoped<IPlayerRatingService<StrikerEntity, RatingEntity>, PlayerRatingService<StrikerEntity, RatingEntity>>();
builder.Services.AddScoped<IStatNormalizerService, StatNormalizerService>();

// MediatR
builder.Services.AddMediatR(typeof(LoadStrikersCommandHandler));

// AutoMapper
builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Controllers + Swagger
builder.Services.AddControllers(); 
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<CreateStrikerCommandValidator>();
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

// CORS configuration
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy
            .WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// DB Connection
var connectionString = Environment.GetEnvironmentVariable("MYSQL_CONNECTION_STRING");
if (string.IsNullOrWhiteSpace(connectionString))
    throw new InvalidOperationException("MYSQL_CONNECTION_STRING is not set in the environment variables (In Run Time).");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 36))));

// Build app
var app = builder.Build();

// Validation middleware
app.UseMiddleware<ValidationExceptionMiddleware>();

// Use CORS (must be before MapControllers)
app.UseCors();

// Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();
