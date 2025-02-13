using Application.Contracts.Generic;
using Application.Features.Lookups.MainCategories.Commands.Create;
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

    //[HttpGet("{id}")]
    //public async Task<ResponseViewModel<MainCategoryViewModel>> GetAsync(int id) => await Mediator.Send(new GetMainCategoryQuery() { Id = id });

    //[HttpGet("GetList")]
    //public async Task<ResponseViewModel<List<MainCategoryViewModel>>> GetAsync() => await Mediator.Send(new GetMainCategoriesQuery());

    //[HttpGet("GetActionStatusList")]
    //public async Task<ResponseViewModel<List<MainCategoryViewModel>>> GetListAsync(ActionStatus actionStatusId) => await Mediator.Send(new GetMainCategoriesByStatusQuery() { Status = actionStatusId });

    [HttpPost("Create")]
    public async Task<ResponseViewModel<MainCategoryViewModel>> PostAsync([FromBody] CreateMainCategoryCommand model) => await Mediator.Send(model);

    //[HttpPut("Update")]
    //public async Task<ResponseViewModel<MainCategoryViewModel>> PutAsync([FromBody] UpdateMainCategoryCommand model) => await Mediator.Send(model);

    //[HttpPut("Active")]
    //public async Task<ResponseViewModel<MainCategoryViewModel>> ActiveAsync([FromBody] ActiveMainCategoryCommand model) => await Mediator.Send(model);

    //[HttpPut("Recover")]
    //public async Task<ResponseViewModel<MainCategoryViewModel>> RecoverAsync([FromBody] RecoverMainCategoryCommand model) => await Mediator.Send(model);

    //[HttpPut("Restore")]
    //public async Task<ResponseViewModel<MainCategoryViewModel>> RestoreAsync([FromBody] RestoreMainCategoryCommand model) => await Mediator.Send(model);

    //[HttpDelete("Delete")]
    //public async Task<ResponseViewModel<MainCategoryViewModel>> DeleteAsync([FromBody] DeleteMainCategoryCommand model) => await Mediator.Send(model);

    //[HttpDelete("Archive")]
    //public async Task<ResponseViewModel<MainCategoryViewModel>> ArchiveAsync([FromBody] ArchiveMainCategoryCommand model) => await Mediator.Send(model);
    //[HttpPut("Upload")]
    //public async Task<ResponseViewModel<MainCategoryViewModel>> UploadAsync([FromForm] UploadMainCategoryCommand model) => await Mediator.Send(model);

    //[HttpPost("Download")]
    //public async Task<IActionResult?> DowonlodAsync(DownloadMainCategoryCommand command)
    //{
    //    var file = await Mediator.Send(command);
    //    if (file.Status != FeedBackCode.FileDownloaded)
    //        return BadRequest(file.Status);

    //    return File(file.Success!.Contents!, file.Success.Type!, file.Success.Name);
    //}
}
