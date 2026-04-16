using Kakushkin_NewsFeed.Host.Extensions;

namespace Kakushkin_NewsFeed.Host;

public class Startup
{
    public IConfiguration Configuration { get; }
    public IHostApplicationBuilder Builder { get; }

    public Startup(IHostApplicationBuilder builder, IConfiguration configuration)
    {
        Builder = builder;
        Configuration = configuration;
    }

    public void ConfigureBuilder()
    {
        Builder.AddLogging();
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddAutoMapper();
        services.AddSwagger();
        services.AddJwtAuthentication(Configuration);
        services.AddApplicationServices();
        services.AddMediatRServices();
        services.AddPersistence(Configuration, Builder.Environment);
        services.AddGlobalExceptionHandler();
        services.AddAuthorization();
    }

    public void ConfigureMiddleware(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseRouting();
        app.UseCors("AllowFrontend");
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        app.UseExceptionHandler();
        
    }
}
