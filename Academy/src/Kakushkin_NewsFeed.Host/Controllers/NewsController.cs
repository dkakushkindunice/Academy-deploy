using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text;
using Kakushkin_NewsFeed.Application.News.Commands;
using Kakushkin_NewsFeed.Application.News.Dto;
using Kakushkin_NewsFeed.Application.News.Queries;
using Kakushkin_NewsFeed.Host.Dto;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kakushkin_NewsFeed.Host.Controllers;

[ApiController]
[Authorize]
[Route("v1/news")]
public class NewsController : ControllerBase
{
    private readonly IMediator _mediator;

    public NewsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<ApiResponse<PaginatedNewsDto>>> GetPaginatedNews(
        [FromQuery, Required] int limit,
        [FromQuery, Required] int offset,
        CancellationToken cancellationToken)
    {
        var query = new GetPaginatedNewsQuery
        {
            Limit = limit,
            Offset = offset,
        };

        var result = await _mediator.Send(query, cancellationToken);

        if (!result.Success)
        {
            return BadRequest(result.Errors.First());
        }

        return Ok(result.Data);
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<NewsOutDto>>> CreateNews(
        CreateNewsCommand command,
        CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        command.AuthorId = Guid.Parse(userId);

        var result = await _mediator.Send(command, cancellationToken);

        if (!result.Success)
        {
            return BadRequest(result.Errors.First());
        }

        return Ok(result.Data);
    }

    [HttpPut("{id:long}")]
    public async Task<ActionResult<ApiResponse>> UpdateNews(
        [FromRoute] long id,
        PutNewsCommand command,
        CancellationToken cancellationToken)
    {
        command.NewsId = id;    
        var result = await _mediator.Send(command, cancellationToken);

        if (!result.Success)
        {
            return BadRequest(result.Errors.First());
        }

        return Ok(result.Data);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse>> DeleteNews(
        [FromRoute] long id,
        CancellationToken cancellationToken)
    {
        var command = new DeleteNewsCommand
        {
            NewsId = id,
            AuthorId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier))
        };

        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result.Message);
    }

    [HttpGet("find")]
    public async Task<ActionResult<ApiResponse>> GetNewsByFilters(
        [FromQuery] GetNewsByFiltersQuery query,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(query, cancellationToken);

        if (!result.Success)
        {
            return BadRequest(result.Errors.First());
        }

        return Ok(result.Data);
    }

    [HttpGet("user/{userId}")]
    public async Task<ActionResult<ApiResponse>> GetNewsByUser(
        [FromRoute, Required] Guid userId,
        [FromQuery, Required] int limit,
        [FromQuery, Required] int offset,
        CancellationToken cancellationToken)
    {
        var query = new GetNewsByUserQuery
        {
            UserId = userId,
            Limit = limit,
            Offset = offset
        };

        var result = await _mediator.Send(query, cancellationToken);

        if (!result.Success)
        {
            return BadRequest(result.Errors.First());
        }

        return Ok(result.Data);
    }
}
