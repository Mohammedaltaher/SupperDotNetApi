//using Application.Features.Lookups.MainCategories.ViewModels;

//namespace Application.Features.Lookups.MainCategories.Commands.Delete;

//public class DeleteMainCategoryHandler : IRequestHandler<DeleteMainCategoryCommand, ResponseViewModel<DetailMainCategoryViewModel>>
//{
//      private readonly IMapper Mapper; private readonly IUnitOfWork UnitOfWork;
//    private readonly ILocalizer Localizer;
//    public DeleteMainCategoryHandler(IMapper mapper, IUnitOfWork unitofwork, ILocalizer localizer)
//    {
//        Mapper = mapper;
//        UnitOfWork = unitofwork;
//        Localizer = localizer;
//    }

//    public async Task<ResponseViewModel<DetailMainCategoryViewModel>> Handle(DeleteMainCategoryCommand request, CancellationToken cancellationToken)
//    {
//        // Implement the logic for handling the DeleteMainCategoryCommand here

//        if (request.Id is null)
//            return new ResponseViewModel<DetailMainCategoryViewModel>(FeedBackCode.NullOrEmpty);

//        var getRow = await UnitOfWork.MainCategoryService.GetAsync(x => x.Id == request.Id);
//        if (getRow is null)
//            return new ResponseViewModel<DetailMainCategoryViewModel>(FeedBackCode.NullOrEmpty);

//        if(await UnitOfWork.SubCategoryService.GetTotal(x => x.MainCategoryId == getRow.Id) > 0)
//            return new ResponseViewModel<DetailMainCategoryViewModel>(FeedBackCode.NotArchived, Localizer["AlreadyLinked"]);

//        if (await UnitOfWork.ServiceInfoService.GetTotal(x => x.MainCategoryId == getRow.Id) > 0)
//            return new ResponseViewModel<DetailMainCategoryViewModel>(FeedBackCode.NotArchived, Localizer["AlreadyLinked"]);

//        var model = Mapper.Map(request, getRow);
//        var isSaved = await UnitOfWork.MainCategoryService.UpdateAsync(x => x.Id == getRow.Id, model, cancellationToken);
//        if (isSaved != FeedBackCode.Updated)
//            return new ResponseViewModel<DetailMainCategoryViewModel>(FeedBackCode.NotDeleted);

//        var viewModel = Mapper.Map<DetailMainCategoryViewModel>(model);
//        return new ResponseViewModel<DetailMainCategoryViewModel>(FeedBackCode.Deleted, viewModel);

//    }
//}