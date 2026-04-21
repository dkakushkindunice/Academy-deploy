using Kakushkin_NewsFeed.Application.News.Dto;
using Kakushkin_NewsFeed.Application.Tags.Services;
using Kakushkin_NewsFeed.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace Kakushkin_NewsFeed.Application;

public class TagServiceTests
{
    private static ApplicationDbContext MakeDb(out DbContextOptions<ApplicationDbContext> options)
    {
        options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase($"tags_{Guid.NewGuid()}")
            .Options;
        return new ApplicationDbContext(options);
    }

    private static TagService MakeService(ApplicationDbContext db) => new TagService(db);

    [Fact]
    public async Task GetOrCreateUniqueTagsAsync_Should_NormalizeAndCreateMissingTags()
    {
        // Arrange
        using var db = MakeDb(out _);
        db.Tags.Add(new Domain.Models.Tag { Title = "tag1" });
        await db.SaveChangesAsync();

        var service = MakeService(db);
        var input = new List<TagDto>
        {
            new() { Title = "  Tag1 " },
            new() { Title = "TAG2" },
            new() { Title = " tag3  " },
            new() { Title = "tag2" }
        };

        // Act
        var result = await service.GetOrCreateUniqueTagsAsync(input, CancellationToken.None);
        await db.SaveChangesAsync();

        Assert.Equal(3, result.Count);
        Assert.Contains(result, t => t.Title == "tag1");
        Assert.Contains(result, t => t.Title == "tag2");
        Assert.Contains(result, t => t.Title == "tag3");

        var all = db.Tags.OrderBy(t => t.Title).ToList();
        Assert.Equal(new[] { "tag1", "tag2", "tag3" }, all.Select(t => t.Title).ToArray());
    }

    [Fact]
    public async Task GetOrCreateUniqueTagsAsync_Should_NotDuplicateOnSubsequentCalls()
    {
        // Arrange
        using var db = MakeDb(out _);
        var service = MakeService(db);

        var first = new List<TagDto> { new() { Title = "TagA" }, new() { Title = "TagB" } };
        var second = new List<TagDto> { new() { Title = "taga" }, new() { Title = "TAGB" } }; // те же, другой регистр

        // Act
        var r1 = await service.GetOrCreateUniqueTagsAsync(first, CancellationToken.None);
        await db.SaveChangesAsync();

        var r2 = await service.GetOrCreateUniqueTagsAsync(second, CancellationToken.None);
        await db.SaveChangesAsync();

        // Assert
        Assert.Equal(2, r1.Count);
        Assert.Equal(2, r2.Count);

        var all = db.Tags.ToList();
        Assert.Equal(2, all.Count);
        Assert.Contains(all, t => t.Title == "taga");
        Assert.Contains(all, t => t.Title == "tagb");
    }

    [Fact]
    public async Task GetOrCreateUniqueTagsAsync_Should_IgnoreEmptyAndWhitespace()
    {
        // Arrange
        using var db = MakeDb(out _);
        var service = MakeService(db);
        var input = new List<TagDto>
        {
            new() { Title = "" },
            new() { Title = "   " },
            new() { Title = "\t" },
            new() { Title = "Real" },
            new() { Title = "real" }
        };

        // Act
        var result = await service.GetOrCreateUniqueTagsAsync(input, CancellationToken.None);
        await db.SaveChangesAsync();
        // Assert
        Assert.Single(result);
        Assert.Equal("real", result.Single().Title);

        var all = db.Tags.ToList();
        Assert.Single(all);
        Assert.Equal("real", all.Single().Title);
    }

    [Fact]
    public async Task GetOrCreateUniqueTagsAsync_WithNullOrEmptyInputReturnsEmpty()
    {
        using var db = MakeDb(out _);
        var service = MakeService(db);

        var empty = await service.GetOrCreateUniqueTagsAsync(Array.Empty<TagDto>(), CancellationToken.None);
        Assert.Empty(empty);

        var resultNull = await service.GetOrCreateUniqueTagsAsync(null!, CancellationToken.None);
        Assert.Empty(resultNull);
    }
}
