namespace Kakushkin_NewsFeed.Application.News.Dto;

public class PaginatedNewsDto
{
    public ICollection<NewsOutDto> NewsOutDtos { get; set; }
    public int NumberOfElements { get; set; }
}