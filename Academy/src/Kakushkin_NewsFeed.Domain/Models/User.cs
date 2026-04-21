namespace Kakushkin_NewsFeed.Domain.Models;

public class User
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string Name { get; set; }
    public string PasswordHash { get; private set; }
    public string Avatar { get; set; }
    
    public ICollection<News> News { get; set; }
    
    private User() {}
    public static User Create(string email, string name,string avatar)
    {
        return new User
        {
            Email = email,
            Name = name,
            Avatar = avatar
        };
    }
    public void SetPassword(string hashedPassword)
    {
        PasswordHash = hashedPassword;
    }
}
