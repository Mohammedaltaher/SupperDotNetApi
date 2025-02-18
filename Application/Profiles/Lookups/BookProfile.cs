using Application.Features.Lookups.Books.Commands.Create;
using Application.Features.Lookups.Books.Commands.Delete;
using Application.Features.Lookups.Books.Commands.Update;
using Application.Features.Lookups.Books.ViewModels;
using Domain.Entities.Lookups;

namespace Application.Profiles.Lookups;

public class BookProfile : Profile
{
    public BookProfile()
    {

        CreateMap<Book, CreateBookCommand>().ReverseMap().ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        CreateMap<Book, UpdateBookCommand>().ReverseMap().ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        CreateMap<Book, DeleteBookCommand>().ReverseMap().ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        CreateMap<Book, BookViewModel>().ReverseMap();
        CreateMap<Book, ListBookViewModel>().ReverseMap();
    }
}
