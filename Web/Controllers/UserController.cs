using System.Security.Claims;
using Application.Dtos;
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
            return View("Edit", user);
        
        logger.LogError("User with login {login} does not exist", login);
        return ActionResultFactory.CustomServerErrorView(this);

    }
    
    [HttpPost]
    [Route("edit")]
    public async Task<IActionResult> PostEdit(string login, string password, string name, string contact, string defaultAddress)
    {
        if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(name) || string.IsNullOrEmpty(contact))
            return BadRequest();
        
        var oldLogin = HttpContext.User.FindFirst(ClaimTypes.Name)!.Value;
        var user = new UserRequestDto(oldLogin, password, name, contact,
            string.IsNullOrEmpty(defaultAddress) ? null : defaultAddress);
        
        if (await userInteractor.Edit(login, user))
            return Redirect("/user/orders");
        
        return BadRequest();
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
