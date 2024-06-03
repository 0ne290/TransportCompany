using Application.Interactors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

[Authorize(Roles = "User")]
[Route("user/orders")]
public class UserController(UserInteractor userInteractor, ILogger<UserController> logger) : Controller
{
    [HttpGet]
    public IActionResult GetOrder()
    {
        return Ok();
    }

    [HttpPost]
    public IActionResult PostOrder()
    {
        return Ok();
    }
}