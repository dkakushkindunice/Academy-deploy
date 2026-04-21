using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Kakushkin_NewsFeed.Application.Files.Services;

public class LocalFileStorage : IFileStorage
{
    private readonly string _root;
    private static readonly HashSet<string> _allowed = [".png", ".jpg", ".jpeg", ".bmp"];
    private const long MAX_SIZE = 5 * 1024 * 1024;
    private readonly ILogger<LocalFileStorage> _logger;
    
    public LocalFileStorage(IHostEnvironment env, ILogger<LocalFileStorage> logger)
    {
        _logger = logger;
        _root = Path.Combine(env.ContentRootPath ?? "wwwroot", "uploads");
        Directory.CreateDirectory(_root);
        _logger.LogDebug("Upload directory ensured at {Root}.", _root);
    }

    public async Task<string> SaveFileAsync(IFormFile file, CancellationToken ct)
    {
        if (file == null || file.Length == 0)
        {
            _logger.LogWarning("Save aborted: file is null.");
            throw new ArgumentOutOfRangeException("Файл пустой.");
        }

        if (file.Length > MAX_SIZE)
        {
            _logger.LogWarning("Save failed: file too large ({SizeBytes} > {MaxSize}).", file.Length, MAX_SIZE);
            throw new ArgumentOutOfRangeException("Файл слишком большой.");
        }

        var extention = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (!_allowed.Contains(extention))
        {
            _logger.LogWarning("Save failed: disallowed extension {Extension}. Allowed: {Allowed}.", extention, _allowed);
            throw new ArgumentOutOfRangeException("Недопустимый тип файла.");
        }

        var safeName = $"{Guid.NewGuid():N}{extention}";
        var path = Path.Combine(_root, safeName);

        await using var fs = new FileStream(path, FileMode.CreateNew, FileAccess.Write, FileShare.None, 64 * 1024,
            useAsync: true);
        await file.CopyToAsync(fs, ct);

        return path;
    }

    public async Task<(Stream stream, string ContentType)?> GetAsync(string fileName, CancellationToken ct)
    {
        var safe = Path.GetFileName(fileName);
        var path = Path.Combine(_root, safe);
        if (!File.Exists(path)) return null;

        var provider = new Microsoft.AspNetCore.StaticFiles.FileExtensionContentTypeProvider();
        if(!provider.TryGetContentType(path,out var contentType)) contentType = "application/octet-stream";
        
        var stream = new FileStream(path, FileMode.Open, FileAccess.Read,FileShare.Read, 64 * 1024, useAsync: true );
 
        await Task.CompletedTask;
        return (stream, contentType);
    }
}
