using System.Linq.Expressions;
using Application.Features.Lookups.Books.Queries.GetList;
using Application.Features.Lookups.Books.ViewModels;
using Domain.Entities.Lookups;

namespace Invoices.UnitTest.Books.Queries;

public class GetBooksHandlerTests
{
    private readonly Mock<IUnitOfWork> unitOfWorkMock;
    private readonly Mock<IMapper> mapperMock;
    private readonly GetBooksQuery.Handler handler;

    public GetBooksHandlerTests()
    {
        unitOfWorkMock = new Mock<IUnitOfWork>();
        mapperMock = new Mock<IMapper>();
        handler = new GetBooksQuery.Handler(mapperMock.Object, unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_NoFilters_ReturnsPagedBooks()
    {
        // Arrange
        var books = new List<Book> { new Book { Id = Guid.NewGuid().ToString(), Name = "Book 1", NameAr = "كتاب 1" } };
        var bookViewModels = new List<ListBookViewModel> { new ListBookViewModel { Id = books[0].Id, Name = "Book 1", NameAr = "كتاب 1" } };

        unitOfWorkMock.Setup(u => u.Book.GetAllAsync(It.IsAny<QueryParameters<Book>>()))
            .ReturnsAsync(books);
        mapperMock.Setup(m => m.Map<List<ListBookViewModel>>(books)).Returns(bookViewModels);

        // Act
        var result = await handler.Handle(new GetBooksQuery(), CancellationToken.None);

        // Assert
        Assert.Equal(FeedBackCode.OK, result.Status);
        Assert.Equal(bookViewModels, result.Success);
    }

    [Fact]
    public async Task Handle_WithNameFilter_ReturnsFilteredBooks()
    {
        // Arrange
        var books = new List<Book> { new Book { Id = Guid.NewGuid().ToString(), Name = "Filtered Book", NameAr = "كتاب مصفى" } };
        var bookViewModels = new List<ListBookViewModel> { new ListBookViewModel { Id = books[0].Id, Name = "Filtered Book", NameAr = "كتاب مصفى" } };

        unitOfWorkMock.Setup(u => u.Book.GetAllAsync(It.Is<QueryParameters<Book>>(q => q.Filter != null)))
            .ReturnsAsync(books);
        mapperMock.Setup(m => m.Map<List<ListBookViewModel>>(books)).Returns(bookViewModels);

        // Act
        var result = await handler.Handle(new GetBooksQuery { Name = "Filtered" }, CancellationToken.None);

        // Assert
        Assert.Equal(FeedBackCode.OK, result.Status);
        Assert.Equal(bookViewModels, result.Success);
    }

    [Fact]
    public async Task Handle_WithSortByName_ReturnsSortedBooks()
    {
        // Arrange
        var books = new List<Book>
        {
            new Book { Id = Guid.NewGuid().ToString(), Name = "A Book", NameAr = "كتاب أ" },
            new Book { Id = Guid.NewGuid().ToString(), Name = "B Book", NameAr = "كتاب ب" }
        };
        var bookViewModels = books.Select(b => new ListBookViewModel { Id = b.Id, Name = b.Name, NameAr = b.NameAr }).ToList();

        unitOfWorkMock.Setup(u => u.Book.GetAllAsync(It.Is<QueryParameters<Book>>(q => q.OrderBy != null)))
            .ReturnsAsync(books.OrderBy(b => b.Name).ToList());
        mapperMock.Setup(m => m.Map<List<ListBookViewModel>>(It.IsAny<List<Book>>())).Returns(bookViewModels);

        // Act
        var result = await handler.Handle(new GetBooksQuery { SortBy = nameof(Book.Name) }, CancellationToken.None);

        // Assert
        Assert.Equal(FeedBackCode.OK, result.Status);
        Assert.Equal(bookViewModels, result.Success);
        Assert.Equal("A Book", result.Success.First().Name);
    }

    [Fact]
    public async Task Handle_Pagination_WorksCorrectly()
    {
        // Arrange
        var books = Enumerable.Range(1, 20).Select(i => new Book { Id = Guid.NewGuid().ToString(), Name = $"Book {i}", NameAr = $"كتاب {i}" }).ToList();
        var pagedBooks = books.Skip(10).Take(5).ToList();
        var bookViewModels = pagedBooks.Select(b => new ListBookViewModel { Id = b.Id, Name = b.Name, NameAr = b.NameAr }).ToList();

        unitOfWorkMock.Setup(u => u.Book.GetAllAsync(It.Is<QueryParameters<Book>>(q => q.PageIndex == 2 && q.PageSize == 5)))
            .ReturnsAsync(pagedBooks);
        mapperMock.Setup(m => m.Map<List<ListBookViewModel>>(pagedBooks)).Returns(bookViewModels);

        // Act
        var result = await handler.Handle(new GetBooksQuery { PageNumber = 2, PageSize = 5 }, CancellationToken.None);

        // Assert
        Assert.Equal(FeedBackCode.OK, result.Status);
        Assert.Equal(5, result.Success.Count);
        Assert.Equal("Book 11", result.Success.First().Name);
    }
}
