
using System.Linq.Expressions;
using Application.Features.Lookups.Books.Commands.Create;
using Application.Features.Lookups.Books.ViewModels;
using Domain.Entities.Lookups;

namespace Invoices.UnitTest.Books.Commands;

public class CreateBookHandlerTests
{
    private readonly Mock<IUnitOfWork> unitOfWorkMock;
    private readonly Mock<IMapper> mapperMock;
    private readonly CreateBookCommand.Handler handler;

    public CreateBookHandlerTests()
    {
        unitOfWorkMock = new Mock<IUnitOfWork>();
        mapperMock = new Mock<IMapper>();
        handler = new CreateBookCommand.Handler(mapperMock.Object, unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_BookAlreadyExists_ReturnsAlreadyExists()
    {
        // Arrange
        var request = new CreateBookCommand { Name = "Test Book", NameAr = "كتاب الاختبار" };
        var existingBook = new Book { Id = Guid.NewGuid().ToString(), Name = "Test Book", NameAr = "كتاب الاختبار" };

        unitOfWorkMock.Setup(u => u.Book.GetAsync(
            It.IsAny<Expression<Func<Book, bool>>>(), null))
            .ReturnsAsync(existingBook);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(FeedBackCode.AlreadyExists, result.Status);
    }

    [Fact]
    public async Task Handle_SaveChangesFails_ReturnsNotAccept()
    {
        // Arrange
        var request = new CreateBookCommand { Name = "New Book", NameAr = "كتاب جديد" };
        var model = new Book { Id = Guid.NewGuid().ToString() };

        unitOfWorkMock.Setup(u => u.Book.GetAsync(
            It.IsAny<Expression<Func<Book, bool>>>(), null))
            .ReturnsAsync((Book)null);

        mapperMock.Setup(m => m.Map<Book>(request)).Returns(model);
        unitOfWorkMock.Setup(u => u.Book.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(0);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(FeedBackCode.NotAccept, result.Status);
    }

    [Fact]
    public async Task Handle_ValidRequest_CreatesBookAndReturnsOK()
    {
        var id = Guid.NewGuid().ToString();
        // Arrange
        var request = new CreateBookCommand { Name = "Valid Book", NameAr = "كتاب صالح" };
        var bookModel = new Book { Id = id, Name = "Valid Book", NameAr = "كتاب صالح" };
        var bookViewModel = new BookViewModel { Id = id, Name = "Valid Book", NameAr = "كتاب صالح" };

        unitOfWorkMock.Setup(u => u.Book.GetAsync(
            It.IsAny<Expression<Func<Book, bool>>>(), null))
            .ReturnsAsync((Book)null);

        mapperMock.Setup(m => m.Map<Book>(request)).Returns(bookModel);
        unitOfWorkMock.Setup(u => u.Book.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
        mapperMock.Setup(m => m.Map<BookViewModel>(bookModel)).Returns(bookViewModel);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(FeedBackCode.OK, result.Status);
        Assert.Equal(bookViewModel, result.Success);
    }
}
