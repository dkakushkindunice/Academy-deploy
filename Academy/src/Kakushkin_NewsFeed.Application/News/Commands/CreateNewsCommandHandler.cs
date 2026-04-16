using AutoMapper;
using Kakushkin_NewsFeed.Application.News.Dto;
using Kakushkin_NewsFeed.Application.Tags.Services;
using Kakushkin_NewsFeed.Common.Results;
using Kakushkin_NewsFeed.Persistence.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Kakushkin_NewsFeed.Application.News.Commands;

public class CreateNewsCommandHandler : IRequestHandler<CreateNewsCommand, Result<NewsOutDto>>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly ITagService _tagService;
    private readonly ILogger<CreateNewsCommandHandler> _logger;
    public CreateNewsCommandHandler(
        ApplicationDbContext dbContext, 
        IMapper mapper, 
        ITagService tagService, 
        ILogger<CreateNewsCommandHandler> logger)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _tagService = tagService;
        _logger = logger;
    }
    
    public async Task<Result<NewsOutDto>> Handle(CreateNewsCommand request, CancellationToken cancellationToken)
    {
        var uniqueTags = await _tagService.GetOrCreateUniqueTagsAsync(request.Tags, cancellationToken);

        var author = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == request.AuthorId, cancellationToken);

        var news = new Domain.Models.News
        {
            Title = request.Title,
            Description = request.Description,
            Image = request.Image,
            AuthorId = request.AuthorId,
            Author = author,
            Tags = uniqueTags
        };

        await _dbContext.News.AddAsync(news, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        var dto = _mapper.Map<NewsOutDto>(news);

        _logger.LogInformation($"Create new news {news.Title} successfully");
        
        return Result<NewsOutDto>.Ok(dto);
    }
}
