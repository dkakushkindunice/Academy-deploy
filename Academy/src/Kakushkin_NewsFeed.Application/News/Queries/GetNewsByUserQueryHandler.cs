using AutoMapper;
using AutoMapper.QueryableExtensions;
using Kakushkin_NewsFeed.Application.News.Dto;
using Kakushkin_NewsFeed.Common.Results;
using Kakushkin_NewsFeed.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Kakushkin_NewsFeed.Application.News.Queries;

public class GetNewsByUserQueryHandler : IRequestHandler<GetNewsByUserQuery, Result<PaginatedNewsDto>>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly ILogger<GetNewsByUserQueryHandler> _logger;

    public GetNewsByUserQueryHandler(ApplicationDbContext dbContext, IMapper mapper, ILogger<GetNewsByUserQueryHandler> logger)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Result<PaginatedNewsDto>> Handle(GetNewsByUserQuery request, CancellationToken cancellationToken)
    {
        var existingUser = await _dbContext.Users
            .AsNoTracking()
            .AnyAsync(u => u.Id == request.UserId, cancellationToken);

        if (!existingUser)
        {
            _logger.LogWarning("GetNewsByUser failed: user {UserId} not found.", request.UserId);
            return Result<PaginatedNewsDto>.Fail($"Пользователь с id {request.UserId} не найден");
        }

        var newsQuery = _dbContext.News
            .AsNoTracking()
            .Include(n => n.Author)
            .Include(n => n.Tags)
            .Where(n => n.AuthorId == request.UserId)
            .AsQueryable();

        var newsCount = await newsQuery.CountAsync(cancellationToken);

        if (newsCount == 0)
        {
            _logger.LogInformation("GetNewsByUser: user {UserId} has no news.", request.UserId);
            return Result<PaginatedNewsDto>.Fail("У пользователя пока нет новостей");
        }

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

        _logger.LogInformation(
            "GetNewsByUser: returned {Count} of {newsCount} news for user {UserId}.",
            newsList.Count, newsCount, request.UserId);
        
        return Result<PaginatedNewsDto>.Ok(paginatedNewsDto);
    }
}
