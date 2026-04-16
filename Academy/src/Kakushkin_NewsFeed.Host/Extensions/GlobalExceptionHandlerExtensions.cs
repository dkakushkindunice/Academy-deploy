using Kakushkin_NewsFeed.Host.Middleware;

namespace Kakushkin_NewsFeed.Host.Extensions;

public static class GlobalExceptionHandlerExtensions 
{
    public static IServiceCollection AddGlobalExceptionHandler(this IServiceCollection services)
    {
        services.AddProblemDetails();
        services.AddExceptionHandler<GlobalExceptionHandler>();
        
        return services;
    }
}
