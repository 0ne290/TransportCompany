using System.Security.Claims;
using Application.Interactors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

[Authorize(Roles = "User")]
[Route("user")]
public class UserController(UserInteractor userInteractor, ILogger<UserController> logger) : Controller
{
    [HttpGet]
    [Route("orders")]
    public async Task<IActionResult> GetOrders()
    {
        var login = HttpContext.User.FindFirst(ClaimTypes.Name)!.Value;
        
        var orders = await userInteractor.GetAllOrders(login);
        
        return View("Orders", orders);
    }
    
    [HttpGet]
    [Route("create-order")]
    public async Task<IActionResult> GetCreateOrder()
    {
        return View("CreateOrder");
    }

    [HttpPost]
    [Route("create-order")]
    public async Task<IActionResult> PostCreateOrder()
    {
        return Ok();
    }
}