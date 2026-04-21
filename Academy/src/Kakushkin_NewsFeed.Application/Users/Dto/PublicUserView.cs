namespace Kakushkin_NewsFeed.Application.Users.Dto;

public class PublicUserView
{
    public Guid UserId { get; set; }
    public string Avatar { get; set; }
    public string Email { get; set; }
    public string Name { get; set; }
}