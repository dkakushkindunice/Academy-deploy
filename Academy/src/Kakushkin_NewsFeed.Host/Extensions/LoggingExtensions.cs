namespace Kakushkin_NewsFeed.Host.Extensions;

public static class LoggingExtensions 
{
    public static IHostApplicationBuilder AddLogging(this IHostApplicationBuilder builder)
    {
        builder.Logging.ClearProviders();
        builder.Logging.AddConsole();
        
        return builder;
    }
}
