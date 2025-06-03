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