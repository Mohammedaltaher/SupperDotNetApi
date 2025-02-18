namespace Application.Features.Lookups.Books.Commands.Update;


public class UpdateBookCommandValidator : AbstractValidator<UpdateBookCommand>
{
    public UpdateBookCommandValidator()
    {
        RuleFor(command => command.Id)
             .NotEmpty().WithMessage("The Id cannot be empty.")
             .Must(id => Guid.TryParse(id, out _)).WithMessage("The Id must be a valid GUID.");

        RuleFor(command => command.Name)
            .NotEmpty().WithMessage("The Name cannot be empty.")
            .When(command => string.IsNullOrEmpty(command.NameAr)); 

        RuleFor(command => command.NameAr)
            .NotEmpty().WithMessage("The NameAr cannot be empty.")
            .When(command => string.IsNullOrEmpty(command.Name)); 
    }
}
