using Domain.Entities.Lookups;
using Application.Features.Lookups.MainCategories.ViewModels;

namespace Application.Features.Lookups.MainCategories.Commands.Create;

public class CreateMainCategoryCommand : IRequest<ResponseViewModel<MainCategoryViewModel>>
{
    public string? Name { get; set; }
    public string? NameAr { get; set; }

    public class Handler(IMapper mapper, IUnitOfWork unitOfWork) : IRequestHandler<CreateMainCategoryCommand, ResponseViewModel<MainCategoryViewModel>>
    {
        private readonly IMapper _mapper = mapper;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<ResponseViewModel<MainCategoryViewModel>> Handle(CreateMainCategoryCommand request, CancellationToken cancellationToken)
        {
            var existingMainCategory = await _unitOfWork.MainCategory.GetAsync(x => x.NameAr == request.NameAr || x.Name == request.Name);

            if (existingMainCategory != null)
                return new ResponseViewModel<MainCategoryViewModel>(FeedBackCode.AlreadyExists);

            var mainCategoryModel = _mapper.Map<MainCategory>(request);
            _unitOfWork.MainCategory.Insert(mainCategoryModel);

            var isSaved = await _unitOfWork.MainCategory.SaveChangesAsync(cancellationToken);
            if (isSaved == 0)
                return new ResponseViewModel<MainCategoryViewModel>(FeedBackCode.NotAccept);

            var createdMainCategory = await _unitOfWork.MainCategory.GetAsync(x => x.Id == mainCategoryModel.Id);
            return new ResponseViewModel<MainCategoryViewModel>(FeedBackCode.OK, _mapper.Map<MainCategoryViewModel>(createdMainCategory));
        }
    }
}
