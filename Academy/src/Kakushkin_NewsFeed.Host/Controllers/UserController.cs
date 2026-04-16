using System.Security.Claims;
using Kakushkin_NewsFeed.Application.Users.Commands;
using Kakushkin_NewsFeed.Application.Users.Dto;
using Kakushkin_NewsFeed.Application.Users.Queries;
using Kakushkin_NewsFeed.Host.Dto;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kakushkin_NewsFeed.Host.Controllers;

[ApiController]
[Route("v1/user")]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [Authorize]
    [HttpPut]
    public async Task<ActionResult<ApiResponse<PublicUserView>>> ReplaceUserData([FromBody] PutUserCommand command, CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        command.UserId = Guid.Parse(userId);
        var result = await _mediator.Send(command,  cancellationToken);

        if (!result.Success)
            return Forbid();

        return Ok(result.Data);
    }

    [Authorize]
    [HttpDelete]
    public async Task<ActionResult> DeleteUser(CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var command = new DeleteUserCommand { UserId = Guid.Parse(userId) };
        var result = await _mediator.Send(command, cancellationToken);

        return result.Success ? Ok(result) : Forbid();
    }

    [Authorize]
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ApiResponse<PublicUserView>>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var query = new  GetUserByIdQuery { UserId = id };
        var result = await _mediator.Send(query, cancellationToken);

        return result.Success ? Ok(result.Data) : BadRequest(result);
    }

    [Authorize]
    [HttpGet("info")]
    public async Task<ActionResult<ApiResponse<PublicUserView>>> GetCurrentUserInfo(CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var query = new GetUserByIdQuery { UserId = Guid.Parse(userId) };
        var result = await _mediator.Send(query, cancellationToken);

        return result.Success ? Ok(result.Data) : Forbid();
    }
}
