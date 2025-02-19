using System.Linq.Expressions;
using Application.Features.Books.Commands.Delete;
using Application.Features.Books.ViewModels;
using Domain.Entities;

namespace Invoices.UnitTest.Books.Commands;

public class DeleteBookHandlerTests
{
    private readonly Mock<IUnitOfWork> unitOfWorkMock;
    private readonly Mock<IMapper> mapperMock;
    private readonly DeleteBookCommand.Handler handler;

    public DeleteBookHandlerTests()
    {
        unitOfWorkMock = new Mock<IUnitOfWork>();
        mapperMock = new Mock<IMapper>();
        handler = new DeleteBookCommand.Handler(mapperMock.Object, unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_BookNotFound_ReturnsNotFound()
    {
        // Arrange
        var request = new DeleteBookCommand { Id = Guid.NewGuid().ToString() };

        unitOfWorkMock.Setup(u => u.Book.GetAsync(
            It.IsAny<Expression<Func<Book, bool>>>(), null))
            .ReturnsAsync((Book)null);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(FeedBackCode.NotFound, result.Status);
    }

    [Fact]
    public async Task Handle_SaveChangesFails_ReturnsNotDeleted()
    {
        // Arrange
        var id = Guid.NewGuid().ToString();
        var book = new Book { Id = id, Name = "Book to Delete" };

        unitOfWorkMock.Setup(u => u.Book.GetAsync(
            It.IsAny<Expression<Func<Book, bool>>>(), null))
            .ReturnsAsync(book);

        unitOfWorkMock.Setup(u => u.Book.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(0);

        // Act
        var result = await handler.Handle(new DeleteBookCommand { Id = id }, CancellationToken.None);

        // Assert
        Assert.Equal(FeedBackCode.NotDeleted, result.Status);
    }

    [Fact]
    public async Task Handle_ValidRequest_DeletesBookAndReturnsDeleted()
    {
        // Arrange
        var id = Guid.NewGuid().ToString();
        var book = new Book { Id = id, Name = "Valid Book" };
        var bookViewModel = new BookViewModel { Id = id, Name = "Valid Book" };

        unitOfWorkMock.Setup(u => u.Book.GetAsync(
            It.IsAny<Expression<Func<Book, bool>>>(), null))
            .ReturnsAsync(book);

        unitOfWorkMock.Setup(u => u.Book.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        mapperMock.Setup(m => m.Map<BookViewModel>(book)).Returns(bookViewModel);

        // Act
        var result = await handler.Handle(new DeleteBookCommand { Id = id }, CancellationToken.None);

        // Assert
        Assert.Equal(FeedBackCode.Deleted, result.Status);
        Assert.Equal(bookViewModel, result.Success);
    }
}