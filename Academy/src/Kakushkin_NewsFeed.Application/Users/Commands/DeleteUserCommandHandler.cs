using Kakushkin_NewsFeed.Common.Results;
using Kakushkin_NewsFeed.Persistence.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Kakushkin_NewsFeed.Application.Users.Commands;

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Result>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<DeleteUserCommandHandler> _logger;
    public DeleteUserCommandHandler(ApplicationDbContext dbContext, ILogger<DeleteUserCommandHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Result> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(
            u => u.Id == request.UserId, cancellationToken);

        if (user == null)
        {
            _logger.LogWarning("Delete failed: user with ID {UserId} not found.", request.UserId);
            return Result.Fail("Пользователь не найден");
        }

        _dbContext.Users.Remove(user);
        await _dbContext.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation("User with ID {UserId} successfully deleted.", request.UserId);

        return Result.Ok();
    }
}