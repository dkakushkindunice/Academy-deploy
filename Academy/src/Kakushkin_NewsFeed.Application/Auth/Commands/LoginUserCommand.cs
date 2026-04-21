using Kakushkin_NewsFeed.Application.Auth.Dto;
using Kakushkin_NewsFeed.Common.Results;
using MediatR;

namespace Kakushkin_NewsFeed.Application.Auth.Commands;

public class LoginUserCommand : IRequest<Result<UserResponse>>
{
 public string Email { get; set; } = default!;
 public string Password { get; set; } = default!;
}