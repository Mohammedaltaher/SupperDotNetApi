using Application.Features.Lookups.MainCategories.ViewModels;

namespace Application.Features.Lookups.MainCategories.Queries.GetList;

//[Cache(60)]
public class ListMainCategoryQuery : IRequest<ResponseViewModel<List<ListMainCategoryViewModel>>>
{
}