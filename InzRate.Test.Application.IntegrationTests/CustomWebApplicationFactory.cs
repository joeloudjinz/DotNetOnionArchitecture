using InzRate.App.Api;
using InzRate.Core.Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace InzRate.Test.Application.IntegrationTests;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    // A single, shared connection is required for SQLite in-memory databases.
    // EF Core will open and close connections, but the in-memory database is destroyed
    // when the last connection to it is closed. This ensures it stays alive.
    private readonly SqliteConnection _dbConnection;

    public CustomWebApplicationFactory()
    {
        // Using "DataSource=:memory:" with a shared, open connection is the modern
        // and most reliable way to handle SQLite in-memory for testing.
        _dbConnection = new SqliteConnection("DataSource=:memory:");
        _dbConnection.Open();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Find and remove the original DbContext registration.
            var dbContextDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));
            if (dbContextDescriptor != null)
            {
                services.Remove(dbContextDescriptor);
            }

            // Add a new DbContext registration that uses our in-memory SQLite connection.
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlite(_dbConnection);
            });
        });
    }

    public async Task InitializeAsync()
    {
        // Accessing the 'Services' property triggers the host creation.
        // We create a scope to resolve the DbContext.
        using var scope = Services.CreateScope();
        var scopedServices = scope.ServiceProvider;
        var context = scopedServices.GetRequiredService<AppDbContext>();

        // This is where the database schema is created. Because this runs in InitializeAsync,
        // it's guaranteed to happen before any test in the class runs.
        await context.Database.EnsureCreatedAsync();
    }

    // We use 'new' to hide the base DisposeAsync and provide our own implementation
    // that also handles the database connection.
    public new async Task DisposeAsync()
    {
        // It's good practice to also ensure the database is deleted.
        using var scope = Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        await context.Database.EnsureDeletedAsync();

        // Close and dispose of the connection to clean up resources.
        await _dbConnection.CloseAsync();
        await _dbConnection.DisposeAsync();
        
        // It's important to call the base DisposeAsync to allow the factory to clean up.
        await base.DisposeAsync();
    }
}