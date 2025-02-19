namespace Application.Features.Books.Commands.Delete;

public class DeleteBookCommandValidator : AbstractValidator<DeleteBookCommand>
{
    public DeleteBookCommandValidator()
    {
        RuleFor(command => command.Id)
             .NotEmpty().WithMessage("The Id cannot be empty.")
             .Must(id => Guid.TryParse(id, out _)).WithMessage("The Id must be a valid GUID.");
    }
}
