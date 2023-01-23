using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_rpg.Controllers.Abstractions;

public abstract class UserContextController : ControllerBase
{
    protected int GetUserId()
    {
        return int.Parse(HttpContext.User
            .FindFirstValue(ClaimTypes.NameIdentifier));
    }
}
