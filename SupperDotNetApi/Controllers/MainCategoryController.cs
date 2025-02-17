using Application.Contracts.Generic;
using Application.Features.Lookups.MainCategories.Commands.Create;
using Application.Features.Lookups.MainCategories.Commands.Delete;
using Application.Features.Lookups.MainCategories.Commands.Update;
using Application.Features.Lookups.MainCategories.Queries.GetDetail;
using Application.Features.Lookups.MainCategories.Queries.GetList;
using Application.Features.Lookups.MainCategories.ViewModels;
using InvoiceManagement.API.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MainCategoryManagement.API.Controllers;

public class MainCategoriesController : BaseController
{
    public MainCategoriesController(IMediator mediator, IConfiguration configuration) : base(mediator, configuration)
    {
    }

    [HttpGet("{id}")]
    public async Task<ResponseViewModel<MainCategoryViewModel>> GetAsync(int id) => await Mediator.Send(new GetMainCategoryDetailsQuery() { Id = id });

    [HttpGet("GetList")]
    public async Task<ResponseViewModel<List<ListMainCategoryViewModel>>> GetAllAsync(
     [FromQuery] string? Name,
     [FromQuery] string? NameAr,
     [FromQuery] int pageNumber = 1,
     [FromQuery] int pageSize = 10,
     [FromQuery] string? sortBy = "Name")
    {

        return await Mediator.Send(new GetMainCategoriesQuery
        {
            Name = Name,
            NameAr = NameAr,
            PageNumber = pageNumber,
            PageSize = pageSize,
            SortBy = sortBy
        });
    }


    [HttpPost("Create")]
    public async Task<ResponseViewModel<MainCategoryViewModel>> PostAsync([FromBody] CreateMainCategoryCommand model) => await Mediator.Send(model);

    [HttpPut("Update")]
    public async Task<ResponseViewModel<MainCategoryViewModel>> PutAsync([FromBody] UpdateMainCategoryCommand model) => await Mediator.Send(model);

    [HttpDelete("Delete")]
    public async Task<ResponseViewModel<MainCategoryViewModel>> DeleteAsync([FromBody] DeleteMainCategoryCommand model) => await Mediator.Send(model);


}
