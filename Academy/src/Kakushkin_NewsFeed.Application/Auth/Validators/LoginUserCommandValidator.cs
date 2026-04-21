using FluentValidation;
using Kakushkin_NewsFeed.Application.Auth.Commands;

namespace Kakushkin_NewsFeed.Application.Auth.Validators;

public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
{
    public LoginUserCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .MinimumLength(3).WithMessage("Email должен быть больше 3 символов")
            .MaximumLength(100).WithMessage("Email должен быть меньше 100 символов")
            .EmailAddress().WithMessage("Неверный формат email");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Пароль обязателен");
    }
}
