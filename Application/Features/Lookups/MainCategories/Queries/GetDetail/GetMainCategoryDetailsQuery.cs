using Application.Features.Lookups.MainCategories.ViewModels;

namespace Application.Features.Lookups.MainCategories.Queries.GetDetail;

public class GetMainCategoryDetailsQuery : IRequest<ResponseViewModel<MainCategoryViewModel>>
{
    public int Id { get; set; }

    public class Handler(IMapper mapper, IUnitOfWork unitOfWork) : IRequestHandler<GetMainCategoryDetailsQuery, ResponseViewModel<MainCategoryViewModel>>
    {
        private readonly IMapper _mapper = mapper;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<ResponseViewModel<MainCategoryViewModel>> Handle(GetMainCategoryDetailsQuery request, CancellationToken cancellationToken)
        {
            var mainCategory = await _unitOfWork.MainCategory.GetAsync(x => x.Id == request.Id);
            if (mainCategory == null)
                return new ResponseViewModel<MainCategoryViewModel>(FeedBackCode.NotFound);

            var mainCategoryViewModel = _mapper.Map<MainCategoryViewModel>(mainCategory);

            return new ResponseViewModel<MainCategoryViewModel>(FeedBackCode.OK, mainCategoryViewModel);
        }
    }
}
