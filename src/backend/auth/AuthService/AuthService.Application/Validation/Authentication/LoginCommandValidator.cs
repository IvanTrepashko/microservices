using AuthService.Application.Commands.Authentication;
using FluentValidation;

namespace AuthService.Application.Validation.Authentication;

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        // todo: add error pairs
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .WithMessage("Invalid email address")
            .WithErrorCode("Email.Invalid");

        RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required");
    }
}