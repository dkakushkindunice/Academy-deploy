namespace Kakushkin_NewsFeed.Domain.Models;

public class Tag
{
    public long Id { get; set; }
    public string Title { get; set; }
    
    public ICollection<News> News { get; set; } = [];
}
