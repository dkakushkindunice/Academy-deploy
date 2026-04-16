using Kakushkin_NewsFeed.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace Kakushkin_NewsFeed.Host.Extensions;

public static class PersistenceExtensions
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration,
        IHostEnvironment environment)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            string connectionString = configuration.GetConnectionString("DefaultConnection");
            
            options.UseNpgsql(connectionString,
                b => b.CommandTimeout(15).EnableRetryOnFailure(5, TimeSpan.FromSeconds(2), null));
            
            if (environment.IsDevelopment() || environment.IsEnvironment("TEST"))
            {
                options.EnableDetailedErrors();
                options.EnableSensitiveDataLogging();
            } 
        });

        return services;
    }   
}
