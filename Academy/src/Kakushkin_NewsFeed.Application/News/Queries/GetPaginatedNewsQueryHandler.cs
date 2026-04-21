using AutoMapper;
using Kakushkin_NewsFeed.Application.News.Dto;
using Kakushkin_NewsFeed.Common.Results;
using Kakushkin_NewsFeed.Persistence.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Kakushkin_NewsFeed.Application.News.Queries;

public class GetPaginatedNewsQueryHandler : IRequestHandler<GetPaginatedNewsQuery, Result<PaginatedNewsDto>>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly ILogger<GetPaginatedNewsQueryHandler> _logger;
    public GetPaginatedNewsQueryHandler(ApplicationDbContext dbContext, IMapper mapper, ILogger<GetPaginatedNewsQueryHandler> logger)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Result<PaginatedNewsDto>> Handle(
        GetPaginatedNewsQuery request,
        CancellationToken cancellationToken)
    {
        var news = await _dbContext.News
            .Include(n => n.Tags)
            .AsNoTracking()
            .Include(n => n.Author)
            .Skip(request.Offset)
            .Take(request.Limit)
            .ToListAsync(cancellationToken);
    
        var newsCount = await _dbContext.News.CountAsync(cancellationToken: cancellationToken);

        if (news.Count == 0)
        {
            _logger.LogInformation("No news found for the given offset {Offset} and limit {Limit}.",
                request.Offset, request.Limit);
        }
        else
        {
            _logger.LogInformation("Returned {Count} of {Total} total news items.",
                news.Count, newsCount);
        }
        
        var paginatedNewsDto = new PaginatedNewsDto()
        {
            NewsOutDtos = _mapper.Map<List<NewsOutDto>>(news),
            NumberOfElements = newsCount,
        };

        return Result<PaginatedNewsDto>.Ok(paginatedNewsDto);
    }
}