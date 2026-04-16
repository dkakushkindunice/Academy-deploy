using Kakushkin_NewsFeed.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace Kakushkin_NewsFeed.Host.Extensions;

public static class MigrationAndSeedExtensions
{
    public static async Task AddMigrationAndSeedAsync(WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var env = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            if (env.IsDevelopment() || env.IsEnvironment("TEST"))
            {
                await db.Database.MigrateAsync();
                //await Seed.RunAsync(db); // фейковые данные ( потом сделать)
            }
        }
    }
}
