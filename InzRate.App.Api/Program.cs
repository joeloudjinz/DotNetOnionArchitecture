using InzRate.Core.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();
builder.Services.RegisterPersistenceServices(builder.Configuration);
builder.Services.RegisterIdentityServices(builder.Configuration);
builder.Services.RegisterCoreServices(builder.Configuration);
builder.Services.RegisterCommandsAndQueries(builder.Configuration);

var app = builder.Build();
app.Services.InitializeServices(app.Configuration);
app.MapOpenApi();

app.Run();

// --- Testability Hook ---
// This makes the auto-generated Program class public and available to your test project.
// WebApplicationFactory<Program> uses this as its entry point.
namespace InzRate.App.Api
{
    public partial class Program
    {
    }
}