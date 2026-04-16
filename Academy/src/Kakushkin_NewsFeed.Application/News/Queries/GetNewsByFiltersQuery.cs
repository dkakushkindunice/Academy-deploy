namespace Kakushkin_NewsFeed.Application.News.Queries;

public class GetNewsByFiltersQuery : GetPaginatedNewsQuery
{
    public string? Author { get; set; }
    public string? Keywords { get; set; }
    public List<string>? Tags { get; set; }
}