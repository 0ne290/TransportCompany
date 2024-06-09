using System.Security.Claims;
using Application.Interactors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.ActionResults;

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
    [Route("edit")]
    public async Task<IActionResult> GetEdit()
    {
        var login = HttpContext.User.FindFirst(ClaimTypes.Name)!.Value;
        
        var user = await userInteractor.GetUser(login);
        
        if (user != null)
            return View("Orders", orders);
        
        logger.LogError("User with login {login} does not exist", login);
        return ActionResultFactory.CustomServerErrorView(this);

    }
    
    [HttpPost]
    [Route("edit")]
    public async Task<IActionResult> PostEdit()
    {
        var login = HttpContext.User.FindFirst(ClaimTypes.Name)!.Value;
        
        var orders = await userInteractor.GetAllOrders(login);
        
        return View("Orders", orders);
    }
    
    [HttpGet]
    [Route("create-order")]
    public async Task<IActionResult> GetCreateOrder()
    {
        var login = HttpContext.User.FindFirst(ClaimTypes.Name)!.Value;

        var defaultAddress = await userInteractor.GetDefaultAddress(login);
    
        return View("CreateOrder", defaultAddress);
    }

    [HttpPost]
    [Route("create-order")]
    public async Task<IActionResult> PostCreateOrder()
    {
        return Ok();
    }
}
