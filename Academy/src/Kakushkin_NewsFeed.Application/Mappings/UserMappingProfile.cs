using AutoMapper;
using Kakushkin_NewsFeed.Application.News.Dto;
using Kakushkin_NewsFeed.Application.Users.Dto;
using Kakushkin_NewsFeed.Domain.Models;

namespace Kakushkin_NewsFeed.Application.Mappings;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<User, PublicUserView>();
 }
}
