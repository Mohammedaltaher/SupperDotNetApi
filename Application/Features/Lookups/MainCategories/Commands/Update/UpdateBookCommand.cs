using Application.Features.Lookups.Books.ViewModels;

namespace Application.Features.Lookups.Books.Commands.Update;

public class UpdateBookCommand : IRequest<ResponseViewModel<BookViewModel>>
{
    public string Id { get; set; } = string.Empty;
    public string? Name { get; set; }
    public string? NameAr { get; set; }

    public class Handler(IMapper mapper, IUnitOfWork unitOfWork) : IRequestHandler<UpdateBookCommand, ResponseViewModel<BookViewModel>>
    {
        private readonly IMapper _mapper = mapper;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<ResponseViewModel<BookViewModel>> Handle(UpdateBookCommand request, CancellationToken cancellationToken)
        {
            var existingBook = await _unitOfWork.Book.GetAsync(x => x.Id == request.Id);
            if (existingBook == null)
                return new ResponseViewModel<BookViewModel>(FeedBackCode.NotFound);

            existingBook.Name = request.Name ?? existingBook.Name;
            existingBook.NameAr = request.NameAr ?? existingBook.NameAr;

            _unitOfWork.Book.Update(existingBook);

            var isSaved = await _unitOfWork.Book.SaveChangesAsync(cancellationToken);
            if (isSaved == 0)
                return new ResponseViewModel<BookViewModel>(FeedBackCode.NotAccept);

            return new ResponseViewModel<BookViewModel>(FeedBackCode.OK, _mapper.Map<BookViewModel>(existingBook));
        }
    }
}
