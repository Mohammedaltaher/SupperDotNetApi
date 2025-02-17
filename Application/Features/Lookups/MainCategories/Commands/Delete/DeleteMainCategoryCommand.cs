using Application.Features.Lookups.MainCategories.ViewModels;

namespace Application.Features.Lookups.MainCategories.Commands.Delete;

public class DeleteMainCategoryCommand : IRequest<ResponseViewModel<MainCategoryViewModel>>
{
    public int Id { get; set; }

    public class Handler(IMapper mapper, IUnitOfWork unitOfWork) : IRequestHandler<DeleteMainCategoryCommand, ResponseViewModel<MainCategoryViewModel>>
    {
        private readonly IMapper _mapper = mapper;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<ResponseViewModel<MainCategoryViewModel>> Handle(DeleteMainCategoryCommand request, CancellationToken cancellationToken)
        {
            var existingMainCategory = await _unitOfWork.MainCategory.GetAsync(x => x.Id == request.Id);
            if (existingMainCategory == null)
                return new ResponseViewModel<MainCategoryViewModel>(FeedBackCode.NotFound);

            existingMainCategory.IsDeleted = true;

            _unitOfWork.MainCategory.Update(existingMainCategory);

            var isDeleted = await _unitOfWork.MainCategory.SaveChangesAsync(cancellationToken);
            if (isDeleted == 0)
                return new ResponseViewModel<MainCategoryViewModel>(FeedBackCode.NotDeleted);

            return new ResponseViewModel<MainCategoryViewModel>(FeedBackCode.Deleted, _mapper.Map<MainCategoryViewModel>(existingMainCategory));
        }
    }
}
