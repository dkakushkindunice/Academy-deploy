using Kakushkin_NewsFeed.Domain.Models;

namespace Kakushkin_NewsFeed.Application.Auth.Dto;

public class UserResponse
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string Name { get; set; } 
    public string Avatar { get; set; } 
    public string Token { get; set; }

    public UserResponse(User user, string token)
    {
        Id = user.Id;
        Email = user.Email;
        Name = user.Name;
        Avatar = user.Avatar;
        Token = token;
    }
}

