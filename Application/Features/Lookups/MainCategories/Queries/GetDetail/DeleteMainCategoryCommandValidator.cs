namespace Application.Features.Lookups.MainCategories.Queries.GetDetail;

using Application.Features.Lookups.MainCategories.Commands.Delete;

public class DeleteMainCategoryCommandValidator : AbstractValidator<GetMainCategoryDetailsQuery>
{
    public DeleteMainCategoryCommandValidator()
    {
        RuleFor(command => command.Id)
            .GreaterThan(0).WithMessage("The Id must be greater than zero.")
            .NotEmpty().WithMessage("The Id cannot be empty.");
    }
}
