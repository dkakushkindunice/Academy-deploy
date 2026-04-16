namespace Kakushkin_NewsFeed.Domain.Models;

public class News
{
    public long Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Image { get; set; }

    public Guid AuthorId { get; set; }
    public User Author { get; set; }
    public ICollection<Tag> Tags { get; set; } =  [];
}
