using Kakushkin_NewsFeed.Abstractions.Services.Security;
using Kakushkin_NewsFeed.Application.Auth.Dto;
using Kakushkin_NewsFeed.Common.Results;
using Kakushkin_NewsFeed.Domain.Models;
using Kakushkin_NewsFeed.Persistence.Data;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Kakushkin_NewsFeed.Application.Auth.Commands;

public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, Result<UserResponse>>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly ILogger<LoginUserCommandHandler> _logger;

    public LoginUserCommandHandler(ApplicationDbContext dbContext, IPasswordHasher<User> passwordHasher,
        IJwtTokenGenerator jwtTokenGenerator, ILogger<LoginUserCommandHandler> logger)
    {
        _dbContext = dbContext;
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
        _logger = logger;
    }

    public async Task<Result<UserResponse>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);
        if (existingUser == null)
        {
            _logger.LogInformation($"Login failed: user with email {request.Email} not found");
            return Result<UserResponse>.Fail($"Пользователь с email {request.Email} не найден");
        }

        var passwordVerificationResult =
            _passwordHasher.VerifyHashedPassword(existingUser, existingUser.PasswordHash, request.Password);
        if (passwordVerificationResult != PasswordVerificationResult.Success)
        {
            _logger.LogInformation($"Login failed: password verification failed");
            return Result<UserResponse>.Fail("Неверный пароль");
        }

        var token = _jwtTokenGenerator.GenerateToken(existingUser.Id);
        _logger.LogInformation($"Login succeeded: user with email {request.Email} created token {token}");
        var response = new UserResponse(existingUser);

        return Result<UserResponse>.Ok(response);
    }
}
    
