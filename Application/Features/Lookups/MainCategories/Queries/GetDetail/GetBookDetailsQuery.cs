using Application.Features.Lookups.Books.ViewModels;

namespace Application.Features.Lookups.Books.Queries.GetDetail;

public class GetBookDetailsQuery : IRequest<ResponseViewModel<BookViewModel>>
{
    public string Id { get; set; } = string.Empty;

    public class Handler(IMapper mapper, IUnitOfWork unitOfWork) : IRequestHandler<GetBookDetailsQuery, ResponseViewModel<BookViewModel>>
    {
        private readonly IMapper _mapper = mapper;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<ResponseViewModel<BookViewModel>> Handle(GetBookDetailsQuery request, CancellationToken cancellationToken)
        {
            var Book = await _unitOfWork.Book.GetAsync(x => x.Id == request.Id);
            if (Book == null)
                return new ResponseViewModel<BookViewModel>(FeedBackCode.NotFound);

            var BookViewModel = _mapper.Map<BookViewModel>(Book);

            return new ResponseViewModel<BookViewModel>(FeedBackCode.OK, BookViewModel);
        }
    }
}
