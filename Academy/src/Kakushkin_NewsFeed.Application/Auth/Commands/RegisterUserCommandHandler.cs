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

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Result<UserResponse>>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly ILogger<RegisterUserCommandHandler> _logger;
    
    public RegisterUserCommandHandler(
        IPasswordHasher<User> passwordHasher, 
        ApplicationDbContext dbContext, 
        IJwtTokenGenerator jwtTokenGenerator, 
        ILogger<RegisterUserCommandHandler> logger)
    {
        _passwordHasher = passwordHasher;
        _dbContext = dbContext;
        _jwtTokenGenerator = jwtTokenGenerator;
        _logger = logger;
    }

    public async Task<Result<UserResponse>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);

            if (existingUser is not null)
            {
                _logger.LogInformation($"Register failed: user with email {request.Email} already exists");
                return Result<UserResponse>.Fail("Email already exists"); 
            }

            var user = User.Create(
                email: request.Email,
                name: request.Name,
                avatar: request.Avatar
            );

            user.SetPassword(_passwordHasher.HashPassword(user, request.Password));
            
            var createdUser = await _dbContext.Users.AddAsync(user, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            _logger.LogInformation($"Register success: user with email {request.Email} and name {request.Name}");
            
            var token = _jwtTokenGenerator.GenerateToken(createdUser.Entity.Id);
            return Result<UserResponse>.Ok(new UserResponse(user));
    }
}

