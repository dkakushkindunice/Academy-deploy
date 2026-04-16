using AutoMapper;
using FluentAssertions;
using Kakushkin_NewsFeed.Application.News.Commands;
using Kakushkin_NewsFeed.Application.News.Dto;
using Kakushkin_NewsFeed.Application.Tags.Services;
using Kakushkin_NewsFeed.Common.Results;
using Kakushkin_NewsFeed.Domain.Models;
using Kakushkin_NewsFeed.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace Kakushkin_NewsFeed.Application.New;

public class CreateNewsCommandHandlerTests
{
    private readonly IMapper _mapperMock = Substitute.For<IMapper>();
    private readonly ITagService _tagServiceMock = Substitute.For<ITagService>();

    private readonly ILogger<CreateNewsCommandHandler>
        _loggerMock = Substitute.For<ILogger<CreateNewsCommandHandler>>();

    private static CreateNewsCommand MakeCommand(Guid? authorId = null) => new CreateNewsCommand
    {
        Title = "New title",
        Description = "New description",
        Image = "image.png",
        AuthorId = authorId ?? Guid.NewGuid(),
        Tags = new List<TagDto> { new() { Title = "Tag1" }, new() { Title = "Tag2" } }
    };

    private static ApplicationDbContext MakeDb(out DbContextOptions<ApplicationDbContext> options)
    {
        options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase($"CreateNews_{Guid.NewGuid()}")
            .Options;
        return new ApplicationDbContext(options);
    }

    [Fact]
    public async Task Handle_Should_CreateNewsAndReturnSuccess_WithMappedDto()
    {
        // Arrange
        using var db = MakeDb(out _);
        var user = User.Create("u@test.com", "A", "B");
        user.SetPassword("Password123");
        db.Users.Add(user);
        await db.SaveChangesAsync();

        var command = MakeCommand(user.Id);

        var resolvedTags = new List<Domain.Models.Tag>
        {
            new() { Id = 2, Title = "Tag1" },
            new() { Id = 1, Title = "Tag2" }
        };
        _tagServiceMock.GetOrCreateUniqueTagsAsync(command.Tags, Arg.Any<CancellationToken>())
            .Returns(resolvedTags);

        var expectedDto = new NewsOutDto
        {
            Title = command.Title,
            Description = command.Description,
            Image = command.Image,
            UserId = command.AuthorId,
            Tags = new List<TagDto> { new TagDto() { Title = "Tag1" }, new TagDto() { Title = "Tag2" } }
        };
        _mapperMock.Map<NewsOutDto>(Arg.Any<Domain.Models.News>()).Returns(expectedDto);

        var sut = new CreateNewsCommandHandler(db, _mapperMock, _tagServiceMock, _loggerMock);

        // Act
        Result<NewsOutDto> result = await sut.Handle(command, CancellationToken.None);

        // Assert
        result.Success.Should().BeTrue();
        result.Data.Should().BeSameAs(expectedDto);

        var saved = await db.News.Include(n => n.Tags).Include(n => n.Author).SingleAsync();
        saved.Title.Should().Be(command.Title);
        saved.Description.Should().Be(command.Description);
        saved.Image.Should().Be(command.Image);
        saved.AuthorId.Should().Be(command.AuthorId);
        saved.Author.Should().NotBeNull();
        saved.Tags.Select(t => t.Title).Should().BeEquivalentTo("Tag1", "Tag2");

        await _tagServiceMock.Received(1).GetOrCreateUniqueTagsAsync(command.Tags, Arg.Any<CancellationToken>());
        _mapperMock.Received(1).Map<NewsOutDto>(Arg.Any<Domain.Models.News>());
    }

    [Fact]
    public async Task Handle_Should_SaveOnlyUniqueNonEmptyTags()
    {
        // Arrange
        using var db = MakeDb(out _);
        var user = User.Create("u@test.com", "A", "B");
        user.SetPassword("Password123");
        db.Users.Add(user);
        await db.SaveChangesAsync();

        var command = new CreateNewsCommand
        {
            Title = "T",
            Description = "D",
            Image = "img",
            AuthorId = user.Id,
            Tags = new List<TagDto>
            {
                new() { Title = "Tag1" },
                new() { Title = "tag1" },  
                new() { Title = "" },  
                new() { Title = "  " },  
                new() { Title = "Tag2" },
                new() { Title = "Tag2" }  
            }
        };
        
        var unique = new List<Domain.Models.Tag>
        {
            new() { Id = 1, Title = "Tag1" },
            new() { Id = 2, Title = "Tag2" }
        };
        _tagServiceMock.GetOrCreateUniqueTagsAsync(command.Tags, Arg.Any<CancellationToken>())
            .Returns(unique);

        _mapperMock.Map<NewsOutDto>(Arg.Any<Domain.Models.News>()).Returns(new NewsOutDto
            { Title = "T", Description = "D", Image = "img", UserId = user.Id, Tags =new List<TagDto>  { new TagDto() { Title = "Tag1" }, new TagDto() { Title = "Tag2" }  }});

        var sut = new CreateNewsCommandHandler(db, _mapperMock, _tagServiceMock, _loggerMock);

        // Act
        var result = await sut.Handle(command, CancellationToken.None);

        // Assert
        result.Success.Should().BeTrue();

        var saved = await db.News.Include(n => n.Tags).SingleAsync();
        saved.Tags.Select(t => t.Title).Should().BeEquivalentTo("Tag1", "Tag2");

        await _tagServiceMock.Received(1).GetOrCreateUniqueTagsAsync(
            Arg.Is<ICollection<TagDto>>(ts => ts.Count() == 6), 
            Arg.Any<CancellationToken>());
    }
}
