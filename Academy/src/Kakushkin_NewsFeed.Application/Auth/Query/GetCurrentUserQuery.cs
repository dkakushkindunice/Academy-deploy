using Kakushkin_NewsFeed.Application.Auth.Dto;
using Kakushkin_NewsFeed.Common.Results;
using MediatR;

namespace Kakushkin_NewsFeed.Application.Auth.Query;

public class GetCurrentUserQuery : IRequest<Result<UserResponse>>
{
    
}
