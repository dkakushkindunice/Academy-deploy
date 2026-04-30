using System.Security.Claims;
using Kakushkin_NewsFeed.Application.Auth.Dto;
using Kakushkin_NewsFeed.Common.Results;
using Kakushkin_NewsFeed.Persistence.Data;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Kakushkin_NewsFeed.Application.Auth.Query;

public class GetCurrentUserQueryHandler
    : IRequestHandler<GetCurrentUserQuery, Result<UserResponse>>
    {
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ApplicationDbContext _dbContext;

    public GetCurrentUserQueryHandler(
        IHttpContextAccessor httpContextAccessor,
        ApplicationDbContext userRepository)
    {
        _httpContextAccessor = httpContextAccessor;
        _dbContext = userRepository;
    }

    public async Task<Result<UserResponse>> Handle(
        GetCurrentUserQuery request,
        CancellationToken cancellationToken)
    {
        var user = _httpContextAccessor.HttpContext?.User;

        if (user == null || !user.Identity?.IsAuthenticated == true)
        {
            return Result<UserResponse>.Fail("Unauthorized");
        }

        string userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId))
        {
            return Result<UserResponse>.Fail("UserId not found");
        }

        var dbUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == Guid.Parse(userId), cancellationToken);

        if (dbUser == null)
        {
            return Result<UserResponse>.Fail("User not found");
        }

        var response = new UserResponse(dbUser);

        return Result<UserResponse>.Ok(response);
    }
}

