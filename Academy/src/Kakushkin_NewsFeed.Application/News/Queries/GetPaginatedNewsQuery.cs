using Kakushkin_NewsFeed.Application.News.Dto;
using Kakushkin_NewsFeed.Common.Results;
using MediatR;

namespace Kakushkin_NewsFeed.Application.News.Queries;

public class GetPaginatedNewsQuery : IRequest<Result<PaginatedNewsDto>>
{
    public int Limit { get; set; }
    public int Offset { get; set; }
}
