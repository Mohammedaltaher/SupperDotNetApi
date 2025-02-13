//using Application.Features.Lookups.MainCategories.ViewModels;

//namespace Application.Features.Lookups.MainCategories.Commands.Update;

//public class UpdateMainCategoryHandler : IRequestHandler<UpdateMainCategoryCommand, ResponseViewModel<DetailMainCategoryViewModel>>
//{
     
//    private readonly IMapper Mapper;
//    private readonly IUnitOfWork UnitOfWork;
//    private readonly IFileUpload FileUpload;
//    public UpdateMainCategoryHandler(  IMapper mapper, IUnitOfWork unitofwork, IFileUpload FileUpload)
//    {
         
//        Mapper = mapper;
//        UnitOfWork = unitofwork;
//        this.FileUpload = FileUpload;
//    }

//    public async Task<ResponseViewModel<DetailMainCategoryViewModel>> Handle(UpdateMainCategoryCommand request, CancellationToken cancellationToken)
//    {
//        var getRow = await UnitOfWork.MainCategoryService.GetAsync(x => x.Id == request.Id);
//        if (getRow is null)
//            return new ResponseViewModel<DetailMainCategoryViewModel>(FeedBackCode.NotFound);
//        var model = Mapper.Map(request, getRow);
//        var isSaved = await UnitOfWork.MainCategoryService.UpdateAsync(x => x.Id == getRow.Id, model, cancellationToken);
//        if (isSaved != FeedBackCode.Updated)
//            return new ResponseViewModel<DetailMainCategoryViewModel>(FeedBackCode.NotUpdated);

//        var viewModel = Mapper.Map<DetailMainCategoryViewModel>(model);
//        return new ResponseViewModel<DetailMainCategoryViewModel>(FeedBackCode.Updated, viewModel);
//    }
//}