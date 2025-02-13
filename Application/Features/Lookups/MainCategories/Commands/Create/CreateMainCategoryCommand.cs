using Application.Features.Lookups.MainCategories.ViewModels;

namespace Application.Features.Lookups.MainCategories.Commands.Create;

public class CreateMainCategoryCommand : IRequest<ResponseViewModel<DetailMainCategoryViewModel>>
{
    public string? Name { get; set; }
    public string? NameAr { get; set; }
}