using Application.Features.Lookups.MainCategories.Commands.Create;
using Application.Features.Lookups.MainCategories.Commands.Delete;
using Application.Features.Lookups.MainCategories.Commands.Update;
using Application.Features.Lookups.MainCategories.ViewModels;
using Domain.Entities.Lookups;

namespace Application.Profiles.Lookups;

public class MainCategoryProfile : Profile
{
    public MainCategoryProfile()
    {

        CreateMap<MainCategory, CreateMainCategoryCommand>().ReverseMap().ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        //CreateMap<MainCategory, UpdateMainCategoryCommand>().ReverseMap().ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        //CreateMap<MainCategory, DeleteMainCategoryCommand>().ReverseMap().ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        CreateMap<MainCategory, DetailMainCategoryViewModel>()
             .ReverseMap();
        CreateMap<MainCategory, ListMainCategoryViewModel>()
             .ReverseMap();
    }
}
