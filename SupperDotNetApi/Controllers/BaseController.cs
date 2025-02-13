using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceManagement.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BaseController : ControllerBase
{
    protected IMediator Mediator;
    protected IConfiguration Configuration;
    public BaseController(IMediator mediator, IConfiguration configuration)
    {
        Mediator = mediator;
        Configuration = configuration;
    }
}
