using Application.Features.Lookups.MainCategories.ViewModels;
using Domain.Entities.Lookups;
using System.Linq.Expressions;

namespace Application.Features.Lookups.MainCategories.Queries.GetList;

//[Cache(60)]
public class GetMainCategoriesQuery : IRequest<ResponseViewModel<List<ListMainCategoryViewModel>>>
{
    public string? Name { get; set; }
    public string? NameAr { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? SortBy { get; set; }

    public class Handler(IMapper mapper, IUnitOfWork unitOfWork) : IRequestHandler<GetMainCategoriesQuery, ResponseViewModel<List<ListMainCategoryViewModel>>>
    {
        private readonly IMapper _mapper = mapper;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<ResponseViewModel<List<ListMainCategoryViewModel>>> Handle(GetMainCategoriesQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<MainCategory, bool>> filter = x => true;
            if (!string.IsNullOrEmpty(request.Name))
                filter = x => x.Name.Contains(request.Name);
            if (!string.IsNullOrEmpty(request.NameAr))
                filter = filter.CombineWithAnd(x => x.NameAr.Contains(request.NameAr));

            Func<IQueryable<MainCategory>, IOrderedQueryable<MainCategory>>? orderBy = null;
            if (request.SortBy == nameof(MainCategory.NameAr))
                orderBy = q => q.OrderBy(x => x.NameAr);
            if (request.SortBy == nameof(MainCategory.Name))
                orderBy = q => q.OrderBy(x => x.Name);


            var mainCategories = await _unitOfWork.MainCategory.GetAllAsync(new QueryParameters<MainCategory>
            {
                PageIndex = request.PageNumber,
                PageSize = request.PageSize,
                Filter = filter,
                OrderBy = orderBy
            });

            var mainCategoryViewModels = _mapper.Map<List<ListMainCategoryViewModel>>(mainCategories);

            return new ResponseViewModel<List<ListMainCategoryViewModel>>(FeedBackCode.OK, mainCategoryViewModels);
        }
    }
}

