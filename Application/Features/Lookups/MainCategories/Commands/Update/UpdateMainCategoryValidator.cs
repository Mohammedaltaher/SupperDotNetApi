namespace Application.Features.Lookups.MainCategories.Commands.Update;


public class UpdateMainCategoryCommandValidator : AbstractValidator<UpdateMainCategoryCommand>
{
    public UpdateMainCategoryCommandValidator()
    {
        RuleFor(command => command.Id)
            .GreaterThan(0).WithMessage("The Id must be greater than zero.")
            .NotEmpty().WithMessage("The Id cannot be empty.");

        RuleFor(command => command.Name)
            .NotEmpty().WithMessage("The Name cannot be empty.")
            .When(command => string.IsNullOrEmpty(command.NameAr)); 

        RuleFor(command => command.NameAr)
            .NotEmpty().WithMessage("The NameAr cannot be empty.")
            .When(command => string.IsNullOrEmpty(command.Name)); 
    }
}
