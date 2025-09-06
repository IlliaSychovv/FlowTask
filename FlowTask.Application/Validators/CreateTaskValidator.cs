using FlowTask.Application.DTO.Task;
using FluentValidation;

namespace FlowTask.Application.Validators;

public class CreateTaskValidator : AbstractValidator<CreateTaskDto>
{
    public CreateTaskValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required")
            .MaximumLength(200).WithMessage("Title must not exceed 100 characters");
        
        RuleFor(x => x.Description)
            .MaximumLength(3000).WithMessage("Description cannot exceed 3000 characters")
            .When(x => !string.IsNullOrEmpty(x.Description));

        RuleFor(x => x.DueDate)
            .GreaterThanOrEqualTo(DateTime.UtcNow).WithMessage("DueDate cannot be in the past")
            .When(x => x.DueDate.HasValue);
    }
}