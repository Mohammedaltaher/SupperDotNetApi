using Application.Contracts.Generic;
using Application.Features.Books.Commands.Create;
using Application.Features.Books.Commands.Delete;
using Application.Features.Books.Commands.Update;
using Application.Features.Books.Queries.GetDetail;
using Application.Features.Books.Queries.GetList;
using Application.Features.Books.ViewModels;
using InvoiceManagement.API.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BookManagement.API.Controllers;

public class BooksController : BaseController
{
    public BooksController(IMediator mediator, IConfiguration configuration) : base(mediator, configuration)
    {
    }

    [HttpGet("{id}")]
    public async Task<ResponseViewModel<BookViewModel>> GetAsync(string id) => await Mediator.Send(new GetBookDetailsQuery() { Id = id });

    [HttpGet("GetList")]
    public async Task<ResponseViewModel<List<ListBookViewModel>>> GetAllAsync(
     [FromQuery] string? Name,
     [FromQuery] string? NameAr,
     [FromQuery] int pageNumber = 1,
     [FromQuery] int pageSize = 10,
     [FromQuery] string? sortBy = "Name")
    {

        return await Mediator.Send(new GetBooksQuery
        {
            Name = Name,
            NameAr = NameAr,
            PageNumber = pageNumber,
            PageSize = pageSize,
            SortBy = sortBy
        });
    }


    [HttpPost("Create")]
    public async Task<ResponseViewModel<BookViewModel>> PostAsync([FromBody] CreateBookCommand model) => await Mediator.Send(model);

    [HttpPut("Update")]
    public async Task<ResponseViewModel<BookViewModel>> PutAsync([FromBody] UpdateBookCommand model) => await Mediator.Send(model);

    [HttpDelete("Delete")]
    public async Task<ResponseViewModel<BookViewModel>> DeleteAsync([FromBody] DeleteBookCommand model) => await Mediator.Send(model);


}
