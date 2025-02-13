//using Application.Features.Lookups.MainCategories.ViewModels;

//namespace Application.Features.Lookups.MainCategories.Queries.GetDetail;

//public class DetailMainCategoryHandler : IRequestHandler<DetailMainCategoryQuery, ResponseViewModel<DetailMainCategoryViewModel>>
//{
//      private readonly IMapper Mapper; private readonly IUnitOfWork UnitOfWork;
//    public DetailMainCategoryHandler(  IMapper mapper, IUnitOfWork unitofwork)
//    {
//          Mapper = mapper; UnitOfWork = unitofwork;
//    }

//    public async Task<ResponseViewModel<DetailMainCategoryViewModel>> Handle(DetailMainCategoryQuery request, CancellationToken cancellationToken)
//    {
//        // Implement the logic for handling the DetailMainCategoryQuery here

//        if (request is null)
//            return new ResponseViewModel<DetailMainCategoryViewModel>(FeedBackCode.NullOrEmpty);

//        if (request.Id is null)
//            return new ResponseViewModel<DetailMainCategoryViewModel>(FeedBackCode.NullOrEmpty);

//        var getRow = await UnitOfWork.MainCategoryService.GetAsync(x => x.Id == request.Id);
//        if (getRow is null)
//            return new ResponseViewModel<DetailMainCategoryViewModel>(FeedBackCode.NotFound);

//        return new ResponseViewModel<DetailMainCategoryViewModel>(FeedBackCode.OK, Mapper.Map<DetailMainCategoryViewModel>(getRow));

//    }
//}