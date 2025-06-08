// Load MYSQL_CONNECTION_STRING before building the application

EnvLoader.LoadRootEnv();

var builder = WebApplication.CreateBuilder(args);

// Register application services
builder.Services.AddScoped<IDataLoadRepository, DataLoadRepository>();

// MediatR
builder.Services.AddMediatR(typeof(LoadStrikersCommandHandler));

// AutoMapper
builder.Services.AddAutoMapper(typeof(Program).Assembly); 

// Mapper Profile
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Services
builder.Services.AddScoped<IStrikerRepository, StrikerRepository>();
builder.Services.AddScoped<IPaginationService, PaginationService>();
builder.Services.AddScoped<IPlayerRatingRepository, PlayerRatingRepository>();
builder.Services.AddScoped<IScoreCalculatorService<StrikerEntity>, PlayerScoreCalculator<StrikerEntity>>();
builder.Services.AddScoped<IPlayerRatingService<StrikerEntity, RatingEntity>, PlayerRatingService<StrikerEntity, RatingEntity>>();
builder.Services.AddScoped<IStatNormalizerService, StatNormalizerService>();


// Swagger + Controllers
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers(); 

// DB Connection
var connectionString = Environment.GetEnvironmentVariable("MYSQL_CONNECTION_STRING");

if (string.IsNullOrWhiteSpace(connectionString))
    throw new InvalidOperationException("MYSQL_CONNECTION_STRING is not set in the environment variables (In Run Time).");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 36))));

var app = builder.Build();

// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers(); 

app.Run();