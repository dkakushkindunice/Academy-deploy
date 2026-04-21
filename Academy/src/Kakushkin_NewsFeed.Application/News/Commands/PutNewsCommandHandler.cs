using AutoMapper;
using Kakushkin_NewsFeed.Application.News.Dto;
using Kakushkin_NewsFeed.Common.Results;
using Kakushkin_NewsFeed.Persistence.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Kakushkin_NewsFeed.Application.News.Commands;

public class PutNewsCommandHandler : IRequestHandler<PutNewsCommand, Result<NewsOutDto>>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly ILogger<PutNewsCommandHandler> _logger;

    public PutNewsCommandHandler(ApplicationDbContext dbContext, IMapper mapper, ILogger<PutNewsCommandHandler> logger)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Result<NewsOutDto>> Handle(PutNewsCommand request, CancellationToken cancellationToken)
    {
        var existingNews = await _dbContext.News.FirstOrDefaultAsync(n => n.Id == request.NewsId, cancellationToken);

        if (existingNews == null)
        {
            _logger.LogError($"News with id {request.NewsId} not found");
            return Result<NewsOutDto>.Fail("Новости с id не существует");
        }

        var updatedNews = _mapper.Map(request, existingNews);
        var dto = _mapper.Map<NewsOutDto>(updatedNews);

        _logger.LogInformation($"Update new news {updatedNews.Title} successfully");
        return Result<NewsOutDto>.Ok(dto);
    }
}
