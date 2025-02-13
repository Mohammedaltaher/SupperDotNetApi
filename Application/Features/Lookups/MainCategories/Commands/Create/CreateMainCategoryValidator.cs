namespace Application.Features.Lookups.MainCategories.Commands.Create;

public class CreateMainCategoryValidator : AbstractValidator<CreateMainCategoryCommand>
{
    public CreateMainCategoryValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters");

        RuleFor(c => c.NameAr)
            .NotEmpty().WithMessage("Arabic name is required")
            .MaximumLength(100).WithMessage("Arabic name must not exceed 100 characters");
    }
}