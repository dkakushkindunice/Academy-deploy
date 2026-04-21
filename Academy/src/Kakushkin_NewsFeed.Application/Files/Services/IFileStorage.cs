using Microsoft.AspNetCore.Http;

namespace Kakushkin_NewsFeed.Application.Files.Services;

public interface IFileStorage
{
    Task<string> SaveFileAsync(IFormFile file, CancellationToken ct);
    Task<(Stream stream, string ContentType)?> GetAsync(string fileName, CancellationToken ct);
}