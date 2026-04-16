using Kakushkin_NewsFeed.Application.Auth.Commands;
using Kakushkin_NewsFeed.Application.Auth.Dto;
using Kakushkin_NewsFeed.Application.Users.Dto;
using Kakushkin_NewsFeed.Host.Dto;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using UserResponse = Kakushkin_NewsFeed.Application.Auth.Dto.UserResponse;

namespace Kakushkin_NewsFeed.Host.Controllers;

[ApiController]
[Route("v1/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("signUp")]
    public async Task<ActionResult<ApiResponse<UserResponse>>> Register([FromBody] RegisterUserCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        
        if (!result.Success)
        {
            return BadRequest(result.Errors.First());
        }

        return Ok(result.Data);
    }
    
    [HttpPost("signIn")]
    public async Task<ActionResult<ApiResponse<UserResponse>>> Login([FromBody] LoginUserCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
    
        if (!result.Success)
        {
            return Unauthorized(result.Errors.First());
        }
    
        return Ok(result.Data);
    }
}
