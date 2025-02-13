using Application.Features.Lookups.MainCategories.ViewModels;

namespace Application.Features.Lookups.MainCategories.Queries.GetDetail;

public class DetailMainCategoryQuery : IRequest<ResponseViewModel<MainCategoryViewModel>>
{
    public string? Id { get; set; }

}