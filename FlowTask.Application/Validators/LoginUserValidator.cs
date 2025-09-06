using FlowTask.Application.DTO.Authorization;
using FluentValidation;

namespace FlowTask.Application.Validators;

public class LoginUserValidator : AbstractValidator<LoginDto>
{
    public LoginUserValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required");
        
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters long")
            .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{6,}$")
            .WithMessage("Password must contain at least one lowercase letter, one uppercase letter, and one number");
    }
}