using Application.Features.Books.ViewModels;
using Domain.Entities;

namespace Application.Features.Books.Commands.Create;

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

            return new ResponseViewModel<BookViewModel>(FeedBackCode.OK, _mapper.Map<BookViewModel>(BookModel));
        }
    }
}
