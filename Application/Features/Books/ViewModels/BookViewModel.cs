namespace Application.Features.Books.ViewModels;

public class BookViewModel
{
    public string Id { get; set; } = string.Empty;
    public string? Name { get; set; }
    public string? NameAr { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
}
