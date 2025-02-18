using Application.Features.Lookups.Books.ViewModels;

namespace Application.Features.Lookups.Books.Commands.Delete;

public class DeleteBookCommand : IRequest<ResponseViewModel<BookViewModel>>
{
    public string Id { get; set; } = string.Empty;


    public class Handler(IMapper mapper, IUnitOfWork unitOfWork) : IRequestHandler<DeleteBookCommand, ResponseViewModel<BookViewModel>>
    {
        private readonly IMapper _mapper = mapper;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<ResponseViewModel<BookViewModel>> Handle(DeleteBookCommand request, CancellationToken cancellationToken)
        {
            var existingBook = await _unitOfWork.Book.GetAsync(x => x.Id == request.Id);
            if (existingBook == null)
                return new ResponseViewModel<BookViewModel>(FeedBackCode.NotFound);

            existingBook.IsDeleted = true;

            _unitOfWork.Book.Update(existingBook);

            var isDeleted = await _unitOfWork.Book.SaveChangesAsync(cancellationToken);
            if (isDeleted == 0)
                return new ResponseViewModel<BookViewModel>(FeedBackCode.NotDeleted);

            return new ResponseViewModel<BookViewModel>(FeedBackCode.Deleted, _mapper.Map<BookViewModel>(existingBook));
        }
    }
}
