using Domain.Entities.Lookups;
using Application.Features.Lookups.MainCategories.ViewModels;
using Domain.Enumerations;

namespace Application.Features.Lookups.MainCategories.Commands.Create;

public class CreateMainCategoryHandler : IRequestHandler<CreateMainCategoryCommand, ResponseViewModel<DetailMainCategoryViewModel>>
{
    private readonly IMapper Mapper;
    private readonly IUnitOfWork UnitOfWork;

    public CreateMainCategoryHandler(IMapper mapper, IUnitOfWork unitofwork)
    {
        Mapper = mapper;
        UnitOfWork = unitofwork;
    }

    public async Task<ResponseViewModel<DetailMainCategoryViewModel>> Handle(CreateMainCategoryCommand request, CancellationToken cancellationToken)
    {
        var getRow = await UnitOfWork.MainCategory.GetAsync(x => x.NameAr == request.NameAr || x.Name == request.Name);

        var model = Mapper.Map<MainCategory>(request);

        UnitOfWork.MainCategory.Insert(model);
        var isSaved = UnitOfWork.MainCategory.SaveChanges();

        if (isSaved == 0)
            return new ResponseViewModel<DetailMainCategoryViewModel>(FeedBackCode.NotAccept);

        var viewModel = await UnitOfWork.MainCategory.GetAsync(x => x.Id == model.Id);
        return new ResponseViewModel<DetailMainCategoryViewModel>(FeedBackCode.OK, Mapper.Map<DetailMainCategoryViewModel>(viewModel));
    }
}