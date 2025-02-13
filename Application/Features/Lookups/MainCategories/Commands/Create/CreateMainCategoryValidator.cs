using FluentValidation;

namespace Application.Features.Lookups.MainCategories.Commands.Create;

public class CreateMainCategoryValidator : AbstractValidator<CreateMainCategoryCommand>
{
    public CreateMainCategoryValidator()
    {
        RuleFor(c => c.Name);
        RuleFor(c => c.NameAr);
    }
}