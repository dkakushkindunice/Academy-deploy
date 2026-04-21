namespace Kakushkin_NewsFeed.Application.News.Queries;

public class GetNewsByUserQuery : GetPaginatedNewsQuery
{
    public Guid UserId { get; set; }
}
