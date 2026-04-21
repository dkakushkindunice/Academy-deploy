using Kakushkin_NewsFeed.Application.Mappings;

namespace Kakushkin_NewsFeed.Host.Extensions;

public static class AutoMapperExtensions
{
    public static IServiceCollection AddAutoMapper(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(UserMappingProfile).Assembly);
        
        return services;
    }
}