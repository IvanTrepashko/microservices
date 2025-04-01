using AuthService.Application.Commands.Authentication;
using FluentValidation;

namespace AuthService.Application.Validation.Authentication;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty();
        RuleFor(x => x.UserName).NotEmpty();
    }
}
