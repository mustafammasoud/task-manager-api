using System.Text.Json;
using System.Text.Json.Serialization;
using TaskManagerApi.Middleware;
using TaskManagerApi.Repositories;
using TaskManagerApi.Services;

var builder = WebApplication.CreateBuilder(args);

// ---------------------------------------------------------------------------
// Services (DI container)
// ---------------------------------------------------------------------------
builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        // Serialize enums as camelCase strings ("pending" / "completed")
        // instead of numeric values.
        options.JsonSerializerOptions.Converters.Add(
            new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Task Manager API",
        Version = "v1",
        Description = "A layered CRUD API for managing daily tasks."
    });
});

// Repository: singleton so the in-memory store persists across requests
// for the app's lifetime.
builder.Services.AddSingleton<ITaskRepository, InMemoryTaskRepository>();

// Service: scoped is fine since it's stateless and only depends on the
// singleton repository.
builder.Services.AddScoped<ITaskService, TaskService>();

var app = builder.Build();

// ---------------------------------------------------------------------------
// Middleware pipeline
// ---------------------------------------------------------------------------
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseSwagger();
app.UseSwaggerUI(); // http://localhost:5048/swagger

app.MapControllers();

app.MapGet("/", () => Results.Ok(new
{
    message = "Task Manager API is running. Visit /swagger for interactive API docs."
})).ExcludeFromDescription();

app.Run();
