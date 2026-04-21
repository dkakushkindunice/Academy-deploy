using System.Text.Json.Serialization;
using Kakushkin_NewsFeed.Application.Users.Dto;
using Kakushkin_NewsFeed.Common.Results;
using MediatR;

namespace Kakushkin_NewsFeed.Application.Users.Commands;

public class PutUserCommand : IRequest<Result<PutUserDto>>
{
    [JsonIgnore]
    public Guid UserId { get; set; }
    public string Avatar { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Name { get; set; } = default!;
}