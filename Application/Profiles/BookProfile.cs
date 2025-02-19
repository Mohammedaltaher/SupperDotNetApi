using Application.Features.Books.Commands.Create;
using Application.Features.Books.Commands.Delete;
using Application.Features.Books.Commands.Update;
using Application.Features.Books.ViewModels;
using Domain.Entities;

namespace Application.Profiles;

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
