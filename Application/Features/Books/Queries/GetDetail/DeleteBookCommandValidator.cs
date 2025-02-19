namespace Application.Features.Books.Queries.GetDetail;
public class DeleteBookCommandValidator : AbstractValidator<GetBookDetailsQuery>
{
    public DeleteBookCommandValidator()
    {
        RuleFor(command => command.Id)
             .NotEmpty().WithMessage("The Id cannot be empty.")
             .Must(id => Guid.TryParse(id, out _)).WithMessage("The Id must be a valid GUID.");
    }
}
