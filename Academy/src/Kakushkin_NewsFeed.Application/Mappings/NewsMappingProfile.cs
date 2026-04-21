using AutoMapper;
using Kakushkin_NewsFeed.Application.News.Commands;
using Kakushkin_NewsFeed.Application.News.Dto;
using Kakushkin_NewsFeed.Domain.Models;

namespace Kakushkin_NewsFeed.Application.Mappings;

public class NewsMappingProfile : Profile
{
    public NewsMappingProfile()
    {
        CreateMap<Tag, TagDto>();
        CreateMap<Domain.Models.News, NewsOutDto>()
            .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags))
            .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Author.Name))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.AuthorId));

        CreateMap<PutNewsCommand, Domain.Models.News>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Image))
            .ForMember(dest => dest.Author, opt => opt.Ignore())
            .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags.Select(t => t.Title)));
    }
}