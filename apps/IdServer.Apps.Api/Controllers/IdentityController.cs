using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdServer.Apps.Api.Controllers;

[ApiController]
[Authorize]
[Route("[controller]")]
public class IdentityController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return new JsonResult(User.Claims.Select(c => new { c.Type, c.Value }));
    }
}
