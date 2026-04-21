using Kakushkin_NewsFeed.Application.News.Dto;
using Kakushkin_NewsFeed.Domain.Models;

namespace Kakushkin_NewsFeed.Application.Tags.Services;

public interface ITagService
{
    Task<ICollection<Tag>> GetOrCreateUniqueTagsAsync(ICollection<TagDto> tags, CancellationToken cancellationToken);
}