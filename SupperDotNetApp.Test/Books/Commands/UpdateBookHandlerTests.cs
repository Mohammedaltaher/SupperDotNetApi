using System.Linq.Expressions;
using Application.Features.Lookups.Books.Commands.Update;
using Application.Features.Lookups.Books.ViewModels;
using Domain.Entities.Lookups;

namespace Invoices.UnitTest.Books.Commands;

public class UpdateBookHandlerTests
{
    private readonly Mock<IUnitOfWork> unitOfWorkMock;
    private readonly Mock<IMapper> mapperMock;
    private readonly UpdateBookCommand.Handler handler;

    public UpdateBookHandlerTests()
    {
        unitOfWorkMock = new Mock<IUnitOfWork>();
        mapperMock = new Mock<IMapper>();
        handler = new UpdateBookCommand.Handler(mapperMock.Object, unitOfWorkMock.Object);
    }

    //[Fact]
    //public async Task Handle_IdIsEmpty_ReturnsValidationError()
    //{
    //    // Arrange
    //    var request = new UpdateBookCommand { Id = string.Empty };

    //    // Act & Assert
    //    var exception = await Assert.ThrowsAsync<ValidationException>(() => handler.Handle(request, CancellationToken.None));
    //    Assert.Contains("The Id cannot be empty.", exception.Message);
    //}

    //[Fact]
    //public async Task Handle_IdIsNotValidGuid_ReturnsValidationError()
    //{
    //    // Arrange
    //    var request = new UpdateBookCommand { Id = "invalid-guid" };

    //    // Act & Assert
    //    var exception = await Assert.ThrowsAsync<ValidationException>(() => handler.Handle(request, CancellationToken.None));
    //    Assert.Contains("The Id must be a valid GUID.", exception.Message);
    //}

    [Fact]
    public async Task Handle_BookNotFound_ReturnsNotFound()
    {
        // Arrange
        var request = new UpdateBookCommand { Id = Guid.NewGuid().ToString() };

        unitOfWorkMock.Setup(u => u.Book.GetAsync(
            It.IsAny<Expression<Func<Book, bool>>>(), null))
            .ReturnsAsync((Book)null);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(FeedBackCode.NotFound, result.Status);
    }

    [Fact]
    public async Task Handle_SaveChangesFails_ReturnsNotAccept()
    {
        // Arrange
        var id = Guid.NewGuid().ToString();
        var book = new Book { Id = id, Name = "Original Name", NameAr = "الاسم الأصلي" };

        unitOfWorkMock.Setup(u => u.Book.GetAsync(
            It.IsAny<Expression<Func<Book, bool>>>(), null))
            .ReturnsAsync(book);

        unitOfWorkMock.Setup(u => u.Book.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(0);

        // Act
        var result = await handler.Handle(new UpdateBookCommand { Id = id, Name = "Updated Name" }, CancellationToken.None);

        // Assert
        Assert.Equal(FeedBackCode.NotAccept, result.Status);
    }

    [Fact]
    public async Task Handle_ValidRequest_UpdatesBookAndReturnsOK()
    {
        // Arrange
        var id = Guid.NewGuid().ToString();
        var book = new Book { Id = id, Name = "Original Name", NameAr = "الاسم الأصلي" };
        var updatedViewModel = new BookViewModel { Id = id, Name = "Updated Name", NameAr = "الاسم المحدث" };

        unitOfWorkMock.Setup(u => u.Book.GetAsync(
            It.IsAny<Expression<Func<Book, bool>>>(), null))
            .ReturnsAsync(book);

        unitOfWorkMock.Setup(u => u.Book.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        mapperMock.Setup(m => m.Map<BookViewModel>(book)).Returns(updatedViewModel);

        // Act
        var result = await handler.Handle(new UpdateBookCommand { Id = id, Name = "Updated Name", NameAr = "الاسم المحدث" }, CancellationToken.None);

        // Assert
        Assert.Equal(FeedBackCode.OK, result.Status);
        Assert.Equal(updatedViewModel, result.Success);
        Assert.Equal("Updated Name", book.Name);
        Assert.Equal("الاسم المحدث", book.NameAr);
    }
}