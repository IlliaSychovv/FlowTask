using FlowTask.Application.DTO.Authorization;
using FluentValidation;

namespace FlowTask.Application.Validators;

public class RegisterUserValidator : AbstractValidator<RegisterDto>
{
    public RegisterUserValidator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("Username is required")
            .MaximumLength(30).WithMessage("Username must maximum 30 characters");
        
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Email is required");
        
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters long")
            .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{6,}$")
            .WithMessage("Password must contain at least one lowercase letter, one uppercase letter, and one number");
    }
}