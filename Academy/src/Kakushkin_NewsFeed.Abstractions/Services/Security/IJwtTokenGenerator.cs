namespace Kakushkin_NewsFeed.Abstractions.Services.Security;

public interface IJwtTokenGenerator
{
    string GenerateToken(Guid userId);
}
