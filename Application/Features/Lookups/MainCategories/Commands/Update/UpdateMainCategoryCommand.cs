using Application.Features.Lookups.MainCategories.ViewModels;

namespace Application.Features.Lookups.MainCategories.Commands.Update;

public class UpdateMainCategoryCommand : IRequest<ResponseViewModel<MainCategoryViewModel>>
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? NameAr { get; set; }

    public class Handler(IMapper mapper, IUnitOfWork unitOfWork) : IRequestHandler<UpdateMainCategoryCommand, ResponseViewModel<MainCategoryViewModel>>
    {
        private readonly IMapper _mapper = mapper;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<ResponseViewModel<MainCategoryViewModel>> Handle(UpdateMainCategoryCommand request, CancellationToken cancellationToken)
        {
            var existingMainCategory = await _unitOfWork.MainCategory.GetAsync(x => x.Id == request.Id);
            if (existingMainCategory == null)
                return new ResponseViewModel<MainCategoryViewModel>(FeedBackCode.NotFound);

            existingMainCategory.Name = request.Name ?? existingMainCategory.Name;
            existingMainCategory.NameAr = request.NameAr ?? existingMainCategory.NameAr;

            _unitOfWork.MainCategory.Update(existingMainCategory);

            var isSaved = await _unitOfWork.MainCategory.SaveChangesAsync(cancellationToken);
            if (isSaved == 0)
                return new ResponseViewModel<MainCategoryViewModel>(FeedBackCode.NotAccept);

            return new ResponseViewModel<MainCategoryViewModel>(FeedBackCode.OK, _mapper.Map<MainCategoryViewModel>(existingMainCategory));
        }
    }
}
