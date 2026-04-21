using Kakushkin_NewsFeed.Application.Auth.Commands;

namespace Kakushkin_NewsFeed.Host.Extensions;

public static class MediatRExtensions
{
    public static IServiceCollection AddMediatRServices(this IServiceCollection services)
    {
        services.AddMediatR(configuration =>
        {
            configuration.RegisterServicesFromAssembly(typeof(RegisterUserCommand).Assembly);
        });

        return services;
    }
}
