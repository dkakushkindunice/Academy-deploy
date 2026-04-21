using FluentValidation;
using Kakushkin_NewsFeed.Application.Users.Commands;

namespace Kakushkin_NewsFeed.Application.Users.Validators;

public class PutUserCommandValidator : AbstractValidator<PutUserCommand>
{
    public PutUserCommandValidator()
    {
        RuleFor(u => u.Email)
            .NotEmpty().WithMessage("Email не должен быть пустым.")
            .MinimumLength(3).WithMessage("Email должен содержать не менее 3 символов.")
            .MaximumLength(100).WithMessage("Email не должен превышать 100 символов.")
            .EmailAddress().WithMessage("Email должен быть в правильном формате.");

        RuleFor(u => u.Name)
            .NotEmpty().WithMessage("Имя не должно быть пустым.")
            .MinimumLength(3).WithMessage("Имя должно содержать не менее 3 символов.")
            .MaximumLength(25).WithMessage("Имя не должно превышать 25 символов.");

        RuleFor(u => u.Avatar)
            .NotEmpty().WithMessage("Ссылка на аватар не должна быть пустой.")
            .MinimumLength(3).WithMessage("Ссылка на аватар должна содержать не менее 3 символов.")
            .MaximumLength(160).WithMessage("Ссылка на аватар не должна превышать 160 символов.");}
}
