using System.Text.Json.Serialization;
using Kakushkin_NewsFeed.Application.News.Dto;
using Kakushkin_NewsFeed.Common.Results;
using MediatR;

namespace Kakushkin_NewsFeed.Application.News.Commands;

public class CreateNewsCommand : IRequest<Result<NewsOutDto>>
{
   public string Title { get; set; } = default!;
   public string Description { get; set; } = default!;
   public ICollection<TagDto> Tags { get; set; } = default!;
   public string Image { get; set; } = default!;
   [JsonIgnore]
   public Guid AuthorId { get; set; }
}