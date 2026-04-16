using FluentValidation;
using Kakushkin_NewsFeed.Abstractions.Services.Security;
using Kakushkin_NewsFeed.Application.Auth.Validators;
using Kakushkin_NewsFeed.Application.Files.Services;
using Kakushkin_NewsFeed.Application.Securety;
using Kakushkin_NewsFeed.Application.Tags.Services;
using Kakushkin_NewsFeed.Common.Results;
using Kakushkin_NewsFeed.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Kakushkin_NewsFeed.Host.Extensions;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<RegisterUserCommandValidator>();
        services.AddSingleton<IFileStorage, LocalFileStorage>();
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
        services.AddScoped<ITagService, TagService>();
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        return services;
    }
}
