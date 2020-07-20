using Microsoft.AspNetCore.Mvc;

namespace Forum.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public abstract class ApiControllerBase : ControllerBase
    {
    }
}