using Kakushkin_NewsFeed.Common.Results;
using Kakushkin_NewsFeed.Persistence.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Kakushkin_NewsFeed.Application.News.Commands;

public class DeleteNewsCommandHandler : IRequestHandler<DeleteNewsCommand, Result>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<DeleteNewsCommandHandler> _logger;

    public DeleteNewsCommandHandler(ApplicationDbContext dbContext, ILogger<DeleteNewsCommandHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }
    
    public async Task<Result> Handle(DeleteNewsCommand request, CancellationToken cancellationToken)
    {
        var news = await _dbContext.News
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.NewsId, cancellationToken);

        if (news == null)
        {
            _logger.LogWarning("Delete failed: news with id {NewsId} not found.", request.NewsId);
            return Result.Fail($"Новость с id {request.NewsId} не найдена");
        }
        
        if (news.AuthorId != request.AuthorId)
        {
            _logger.LogWarning("Delete denied: user {AuthorId} is not the author of news {NewsId}.", request.AuthorId, request.NewsId);
            return Result.Fail("Вы не являетесь автором этой новости");
        }
        
        await _dbContext.News
            .Where(x => x.Id == request.NewsId)
            .ExecuteDeleteAsync(cancellationToken);

        _logger.LogInformation("News {NewsId} deleted by author {AuthorId}.", request.NewsId, request.AuthorId);
        return Result.Ok();
    }
}