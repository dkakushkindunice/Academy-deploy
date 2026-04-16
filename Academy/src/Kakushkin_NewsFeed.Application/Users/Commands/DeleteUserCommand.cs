using Kakushkin_NewsFeed.Common.Results;
using MediatR;

namespace Kakushkin_NewsFeed.Application.Users.Commands;

public class DeleteUserCommand : IRequest<Result>
{
    public Guid UserId { get; set; }
}