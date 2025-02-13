//using Application.Features.Lookups.MainCategories.ViewModels;

//namespace Application.Features.Lookups.MainCategories.Queries.GetList;

//public class ListMainCategoryHandler : IRequestHandler<ListMainCategoryQuery, ResponseViewModel<List<ListMainCategoryViewModel>>>
//{
//    private readonly IMapper Mapper;
//    private readonly IUnitOfWork UnitOfWork;
//    public ListMainCategoryHandler(IMapper mapper, IUnitOfWork unitofwork)
//    {
//        Mapper = mapper;
//        UnitOfWork = unitofwork;
//    }

//    public async Task<ResponseViewModel<List<ListMainCategoryViewModel>>> Handle(ListMainCategoryQuery request, CancellationToken cancellationToken)
//    {
//        var getList = await UnitOfWork.MainCategoryService.GetAllAsync();
//        if (getList != null && !getList.Any())
//            return new ResponseViewModel<List<ListMainCategoryViewModel>>(FeedBackCode.OK, new());

//        return new ResponseViewModel<List<ListMainCategoryViewModel>>(FeedBackCode.OK, Mapper.Map<List<ListMainCategoryViewModel>>(getList));
//    }
//}