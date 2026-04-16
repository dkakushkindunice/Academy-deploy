using Kakushkin_NewsFeed.Application.Files.Services;
using Kakushkin_NewsFeed.Host.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kakushkin_NewsFeed.Host.Controllers;

[ApiController]
[Route("v1/file")]
public class FileController : ControllerBase
{
    private readonly IFileStorage _storage;

    public FileController(IFileStorage storage)
    {
        _storage = storage;
    }

    [Authorize]
    [HttpPost("uploadFile")]
    [RequestSizeLimit(10_000_000)]
    public async Task<ActionResult<ApiResponse<string>>> UploadFile(IFormFile file, CancellationToken ct)
    {
        try
        {
            var path = await _storage.SaveFileAsync(file, ct);
            return Ok(new ApiResponse<string>() { Data = path, Message = "Ok" });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new ApiResponse<string>() { Data = null, Message = ex.Message });
        }
    }

    [HttpGet("{fileName}")]
    public async Task<IActionResult> GetFile([FromRoute] string fileName, CancellationToken ct)
    {
        var result = await _storage.GetAsync(fileName, ct);
        if (result is null)
            return NotFound(new ApiResponse() { Message = "Файл не найден" });

        var (stream, contentType) = result.Value;
        return File(stream, contentType);
    }
}