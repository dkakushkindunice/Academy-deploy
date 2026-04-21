using AutoMapper;
using AutoMapper.QueryableExtensions;
using Kakushkin_NewsFeed.Application.News.Dto;
using Kakushkin_NewsFeed.Common.Results;
using Kakushkin_NewsFeed.Persistence.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Kakushkin_NewsFeed.Application.News.Queries;

public class GetNewsByFiltersQueryHandler : IRequestHandler<GetNewsByFiltersQuery, Result<PaginatedNewsDto>>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly ILogger<GetNewsByFiltersQueryHandler> _logger;
    public GetNewsByFiltersQueryHandler(
        ApplicationDbContext dbContext,
        IMapper mapper,
        ILogger<GetNewsByFiltersQueryHandler> logger)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Result<PaginatedNewsDto>> Handle(GetNewsByFiltersQuery request, CancellationToken cancellationToken)
    {
        var newsQuery = _dbContext.News
            .AsNoTracking()
            .Include(n => n.Author)
            .Include(n => n.Tags)
            .AsQueryable();
        
        if (!string.IsNullOrWhiteSpace(request.Keywords))
        {
            var keywords = request.Keywords.ToLower();
            newsQuery = newsQuery.Where(n =>
                n.Title.ToLower().Contains(keywords) ||
                n.Description.ToLower().Contains(keywords));
        }
        
        if (!string.IsNullOrWhiteSpace(request.Author))
        {
            var authorName = request.Author.ToLower();
            newsQuery = newsQuery.Where(n => n.Author.Name.ToLower().Contains(authorName));
        }
        
        if (request.Tags != null && request.Tags.Any())
        {
            var tagsLower = request.Tags.Select(t => t.ToLower()).ToList();
            newsQuery = newsQuery.Where(n =>
                n.Tags.Any(tag => tagsLower.Contains(tag.Title.ToLower())));
        }
        
        var newsCount = await newsQuery.CountAsync(cancellationToken);
        
        var skip = (request.Offset - 1) * request.Limit;
        
        var newsList = await newsQuery
            .Skip(skip)
            .Take(request.Limit)
            .ProjectTo<NewsOutDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
        
        var paginatedNewsDto = new PaginatedNewsDto()
        {
            NewsOutDtos = _mapper.Map<List<NewsOutDto>>(newsList),
            NumberOfElements = newsCount,
        };
        
        _logger.LogInformation($"Get new {request.Keywords} successfully");
        
        return Result<PaginatedNewsDto>.Ok(paginatedNewsDto);
    }
}