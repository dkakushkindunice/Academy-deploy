using Kakushkin_NewsFeed.Application.News.Dto;
using Kakushkin_NewsFeed.Domain.Models;
using Kakushkin_NewsFeed.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace Kakushkin_NewsFeed.Application.Tags.Services;

public class TagService : ITagService
{
    private readonly ApplicationDbContext _dbContext;

    public TagService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ICollection<Tag>> GetOrCreateUniqueTagsAsync(ICollection<TagDto> tags, CancellationToken cancellationToken)
    {
        if (tags is null || tags.Count == 0)
        {
            return Array.Empty<Tag>();
        }

        var normalized = tags
            .Where(t => t is not null && !string.IsNullOrWhiteSpace(t.Title))
            .Select(t => t.Title.Trim().ToLower())
            .ToList();

        if (normalized.Count == 0)
        {
            return Array.Empty<Tag>();
        }

        var existingTags = await _dbContext.Tags
            .Where(t => normalized.Contains(t.Title.ToLower()))
            .ToListAsync(cancellationToken);

        var newTitles = normalized
            .Except(existingTags.Select(x => x.Title.ToLower()))
            .Distinct();
        
        var newTags = newTitles
            .Select(title => new Tag { Title = title })
            .ToList();

        if (newTags.Any())
        {
            await _dbContext.Tags.AddRangeAsync(newTags, cancellationToken);
        }

        var allTags = existingTags.Concat(newTags).ToList();
        return allTags;
    }
}
