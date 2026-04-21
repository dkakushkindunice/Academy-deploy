using Kakushkin_NewsFeed.Application.Users.Dto;
using Kakushkin_NewsFeed.Common.Results;
using MediatR;

namespace Kakushkin_NewsFeed.Application.Users.Queries;

public class GetUserByIdQuery : IRequest<Result<PublicUserView>>
{
    public Guid UserId { get; set; }
}
