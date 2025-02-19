using Application.Features.Books.ViewModels;
using Application.Utilities;
using Domain.Entities;
using System.Linq.Expressions;

namespace Application.Features.Books.Queries.GetList;

[Cache(60)]
public class GetBooksQuery : IRequest<ResponseViewModel<List<ListBookViewModel>>>
{
    public string? Name { get; set; }
    public string? NameAr { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? SortBy { get; set; }

    public class Handler(IMapper mapper, IUnitOfWork unitOfWork) : IRequestHandler<GetBooksQuery, ResponseViewModel<List<ListBookViewModel>>>
    {
        private readonly IMapper _mapper = mapper;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<ResponseViewModel<List<ListBookViewModel>>> Handle(GetBooksQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<Book, bool>> filter = x => true;
            if (!string.IsNullOrEmpty(request.Name))
                filter = x => x.Name.Contains(request.Name);
            if (!string.IsNullOrEmpty(request.NameAr))
                filter = filter.CombineWithAnd(x => x.NameAr.Contains(request.NameAr));

            Func<IQueryable<Book>, IOrderedQueryable<Book>>? orderBy = null;
            if (request.SortBy == nameof(Book.NameAr))
                orderBy = q => q.OrderBy(x => x.NameAr);
            if (request.SortBy == nameof(Book.Name))
                orderBy = q => q.OrderBy(x => x.Name);


            var Books = await _unitOfWork.Book.GetAllAsync(new QueryParameters<Book>
            {
                PageIndex = request.PageNumber,
                PageSize = request.PageSize,
                Filter = filter,
                OrderBy = orderBy
            });

            var BookViewModels = _mapper.Map<List<ListBookViewModel>>(Books);

            return new ResponseViewModel<List<ListBookViewModel>>(FeedBackCode.OK, BookViewModels);
        }
    }
}

