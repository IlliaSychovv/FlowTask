using FlowTask.Application.DTO.Task;
using FluentValidation;

namespace FlowTask.Application.Validators;

public class UpdateTaskValidator : AbstractValidator<UpdateTaskDto>
{
    public UpdateTaskValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required")
            .MaximumLength(200).WithMessage("Title must not exceed 100 characters");
        
        RuleFor(x => x.Description)
            .MaximumLength(3000).WithMessage("Description cannot exceed 3000 characters")
            .When(x => !string.IsNullOrEmpty(x.Description));
    }
}