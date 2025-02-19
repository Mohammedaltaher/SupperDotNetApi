using System.Linq.Expressions;
using Application.Features.Books.Queries.GetDetail;
using Application.Features.Books.ViewModels;
using Domain.Entities;

namespace Invoices.UnitTest.Books.Queries;

public class GetBookDetailsHandlerTests
{
    private readonly Mock<IUnitOfWork> unitOfWorkMock;
    private readonly Mock<IMapper> mapperMock;
    private readonly GetBookDetailsQuery.Handler handler;

    public GetBookDetailsHandlerTests()
    {
        unitOfWorkMock = new Mock<IUnitOfWork>();
        mapperMock = new Mock<IMapper>();
        handler = new GetBookDetailsQuery.Handler(mapperMock.Object, unitOfWorkMock.Object);
    }

   
    [Fact]
    public async Task Handle_BookNotFound_ReturnsNotFound()
    {
        // Arrange
        var request = new GetBookDetailsQuery { Id = Guid.NewGuid().ToString() };

        unitOfWorkMock.Setup(u => u.Book.GetAsync(
            It.IsAny<Expression<Func<Book, bool>>>(), null))
            .ReturnsAsync((Book)null);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(FeedBackCode.NotFound, result.Status);
    }

    [Fact]
    public async Task Handle_ValidRequest_ReturnsBookDetails()
    {
        // Arrange
        var id = Guid.NewGuid().ToString();
        var book = new Book { Id = id, Name = "Test Book", NameAr = "كتاب الاختبار" };
        var bookViewModel = new BookViewModel { Id = id, Name = "Test Book", NameAr = "كتاب الاختبار" };

        unitOfWorkMock.Setup(u => u.Book.GetAsync(
            It.IsAny<Expression<Func<Book, bool>>>(), null))
            .ReturnsAsync(book);

        mapperMock.Setup(m => m.Map<BookViewModel>(book)).Returns(bookViewModel);

        // Act
        var result = await handler.Handle(new GetBookDetailsQuery { Id = id }, CancellationToken.None);

        // Assert
        Assert.Equal(FeedBackCode.OK, result.Status);
        Assert.Equal(bookViewModel, result.Success);
    }
}
