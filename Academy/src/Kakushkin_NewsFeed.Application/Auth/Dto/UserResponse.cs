using Kakushkin_NewsFeed.Domain.Models;

namespace Kakushkin_NewsFeed.Application.Auth.Dto;

public class UserResponse
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string Name { get; set; } 
    public string Avatar { get; set; } 
    
    public UserResponse(User user)
    {
        Id = user.Id;
        Email = user.Email;
        Name = user.Name;
        Avatar = user.Avatar;
    }
}

