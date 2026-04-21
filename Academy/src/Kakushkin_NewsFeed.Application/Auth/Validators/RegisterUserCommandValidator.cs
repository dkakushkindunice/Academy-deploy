using FluentValidation;
using Kakushkin_NewsFeed.Application.Auth.Commands;

namespace Kakushkin_NewsFeed.Application.Auth.Validators;

public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(u => u.Email)
            .NotEmpty().WithMessage("Email не должен быть пустым.")
            .MinimumLength(3).WithMessage("Email должен содержать не менее 3 символов.")
            .MaximumLength(100).WithMessage("Email не должен превышать 100 символов.")
            .EmailAddress().WithMessage("Email должен быть в правильном формате.");
        
        RuleFor(x=>x.Password)
            .NotEmpty()
            .MinimumLength(3)
            .WithMessage("Password must be at least 3 characters long");
        
        RuleFor(u => u.Name)
            .NotEmpty().WithMessage("Имя не должно быть пустым.")
            .MinimumLength(3).WithMessage("Имя должно содержать не менее 3 символов.")
            .MaximumLength(25).WithMessage("Имя не должно превышать 25 символов.");
        
        RuleFor(x => x.Avatar)
            .NotEmpty();
    }
}
