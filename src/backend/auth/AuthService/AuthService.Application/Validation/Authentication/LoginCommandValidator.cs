using AuthService.Application.Commands.Authentication;
using FluentValidation;

namespace AuthService.Application.Validation.Authentication;

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .WithMessage("Invalid email address")
            .WithErrorCode("");
        RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required");
    }
}
