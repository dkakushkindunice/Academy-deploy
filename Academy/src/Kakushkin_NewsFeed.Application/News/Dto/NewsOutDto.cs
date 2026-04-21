using Kakushkin_NewsFeed.Domain.Models;

namespace Kakushkin_NewsFeed.Application.News.Dto;

public class NewsOutDto
{
    public long Id { get; set; }
    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string Image { get; set; } = default!;
    public IEnumerable<TagDto> Tags { get; set; } = [];
    public Guid UserId { get; set; }
    public string Username { get; set; } = default!;
}