using System.Security.Claims;
using Application.Interactors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

[Authorize(Roles = "User")]
[Route("user/orders")]
public class UserController(UserInteractor userInteractor, ILogger<UserController> logger) : Controller
{
    [HttpGet]
    public async Task<IActionResult> GetOrders()
    {
        var login = HttpContext.User.FindFirst(ClaimTypes.Name)!.Value;
        
        var orders = await userInteractor.GetAllOrders(login);
        
        return View("Orders", orders);
    }

    [HttpPost]
    public async Task<IActionResult> PostOrder()
    {
        return Ok();
    }
}