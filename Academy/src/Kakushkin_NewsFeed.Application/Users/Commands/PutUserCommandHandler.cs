using Kakushkin_NewsFeed.Application.Users.Dto;
using Kakushkin_NewsFeed.Common.Results;
using Kakushkin_NewsFeed.Persistence.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Kakushkin_NewsFeed.Application.Users.Commands;

public class PutUserCommandHandler: IRequestHandler<PutUserCommand, Result<PutUserDto>>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<PutUserCommandHandler> _logger;
    public PutUserCommandHandler(ApplicationDbContext dbContext, ILogger<PutUserCommandHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Result<PutUserDto>> Handle(PutUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);
        
        if (user == null)
        {
            _logger.LogWarning("Update failed: user with ID {UserId} not found.", request.UserId);
            return Result<PutUserDto>.Fail("Пользователь не найден");
        }

        
        user.Avatar = request.Avatar;
        user.Email = request.Email;
        user.Name = request.Name;

        await _dbContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("User with ID {UserId} successfully updated.", request.UserId);

        var updatedUser = new PutUserDto(
            user.Id,
            user.Name,
            user.Email,
            user.Avatar
            );
        
        return Result<PutUserDto>.Ok(updatedUser);
    }
}