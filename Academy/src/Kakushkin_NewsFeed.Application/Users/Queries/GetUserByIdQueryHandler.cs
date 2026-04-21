using AutoMapper;
using Kakushkin_NewsFeed.Application.Users.Dto;
using Kakushkin_NewsFeed.Common.Results;
using Kakushkin_NewsFeed.Persistence.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Kakushkin_NewsFeed.Application.Users.Queries;

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, Result<PublicUserView>>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly ILogger<GetUserByIdQueryHandler> _logger;
    public GetUserByIdQueryHandler(ApplicationDbContext dbContext, IMapper mapper, ILogger<GetUserByIdQueryHandler> logger)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Result<PublicUserView>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

        if (user == null)
        {
            _logger.LogWarning("User with ID {UserId} not found.", request.UserId);
            return Result<PublicUserView>.Fail("Пользователь не найден");
        }

        var dto = _mapper.Map<PublicUserView>(user);

        _logger.LogInformation("User with ID {UserId} successfully retrieved.", request.UserId);
        
        return Result<PublicUserView>.Ok(dto);
    }
}
