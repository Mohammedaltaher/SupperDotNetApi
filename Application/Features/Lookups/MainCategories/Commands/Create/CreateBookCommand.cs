using Domain.Entities.Lookups;
using Application.Features.Lookups.Books.ViewModels;

namespace Application.Features.Lookups.Books.Commands.Create;

public class CreateBookCommand : IRequest<ResponseViewModel<BookViewModel>>
{
    public string? Name { get; set; }
    public string? NameAr { get; set; }

    public class Handler(IMapper mapper, IUnitOfWork unitOfWork) : IRequestHandler<CreateBookCommand, ResponseViewModel<BookViewModel>>
    {
        private readonly IMapper _mapper = mapper;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<ResponseViewModel<BookViewModel>> Handle(CreateBookCommand request, CancellationToken cancellationToken)
        {
            var existingBook = await _unitOfWork.Book.GetAsync(x => x.NameAr == request.NameAr || x.Name == request.Name);

            if (existingBook != null)
                return new ResponseViewModel<BookViewModel>(FeedBackCode.AlreadyExists);

            var BookModel = _mapper.Map<Book>(request);
            _unitOfWork.Book.Insert(BookModel);

            var isSaved = await _unitOfWork.Book.SaveChangesAsync(cancellationToken);
            if (isSaved == 0)
                return new ResponseViewModel<BookViewModel>(FeedBackCode.NotAccept);

            var createdBook = await _unitOfWork.Book.GetAsync(x => x.Id == BookModel.Id);
            return new ResponseViewModel<BookViewModel>(FeedBackCode.OK, _mapper.Map<BookViewModel>(createdBook));
        }
    }
}
