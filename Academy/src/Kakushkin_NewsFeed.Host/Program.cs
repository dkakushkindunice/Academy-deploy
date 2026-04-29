using Kakushkin_NewsFeed.Host;
using Kakushkin_NewsFeed.Persistence.Data;
using Microsoft.EntityFrameworkCore;

var  builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins(
                "https://kakushkin-dima.ru",
                "https://www.kakushkin-dima.ru",
                "http://localhost:5173"
            ).AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials(); // если используешь cookies / auth
    });
});
var startup = new Startup(builder, builder.Configuration);
startup.ConfigureBuilder();
startup.ConfigureServices(builder.Services);

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();
}
startup.ConfigureMiddleware(app, app.Environment);
app.MapGet("/", () => "Hello World!");
app.UseHttpsRedirection();
app.Run();
