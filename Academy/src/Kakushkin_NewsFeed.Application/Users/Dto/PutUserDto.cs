namespace Kakushkin_NewsFeed.Application.Users.Dto;

public class PutUserDto
{
    public Guid UserId { get; set; }  
    public string Avatar { get; set; }
    public string Email { get; set; }
    public string Name { get; set; }

    public PutUserDto(Guid userId, string avatar, string email, string name)
    {
        UserId = userId;
        Avatar = avatar;
        Email = email;
        Name = name;
    }
}