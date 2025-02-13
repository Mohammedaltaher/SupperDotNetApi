//using FluentValidation;

//namespace Application.Features.Lookups.MainCategories.Queries.GetDetail;

//public class DetailMainCategoryValidator : AbstractValidator<DetailMainCategoryQuery>
//{
//    public DetailMainCategoryValidator()
//    {
//        RuleFor(c => c.Id)
//            .NotEmpty().WithMessage(c => $"{nameof(c.Id)} is required.")
//            .NotNull().MaximumLength(250)
//            .Must(FluentValidationUtility.BeValidObjectId).WithMessage(c => $"{nameof(c.Id)} is not a valid ObjectId.");

//    }
//}