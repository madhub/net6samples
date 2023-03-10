using UsingConfigurationOption;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddKeyPerFile("/config",true);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.Configure<ApplicationInfo>(builder.Configuration.GetSection("applicationInfo"));

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.Run();
