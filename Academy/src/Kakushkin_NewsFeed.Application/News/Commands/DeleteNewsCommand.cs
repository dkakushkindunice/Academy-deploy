using Kakushkin_NewsFeed.Common.Results;
using MediatR;

namespace Kakushkin_NewsFeed.Application.News.Commands;

public class DeleteNewsCommand : IRequest<Result>
{
    public long NewsId { get; set; }
    public Guid AuthorId { get; set; }
}