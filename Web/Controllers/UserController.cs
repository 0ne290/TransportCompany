using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

[Route("user")]
public class UserController : Controller
{
    //public UserController(ILogger<LoginController> logger)
    //{
    //    _logger = logger;
    //}
    
    [Authorize(Roles = "User")]
    [HttpGet]
    [Route("home")]
    public IActionResult GetUser()
    {
        return Ok();
    }

    /*[HttpGet]
    [Route("administrator")]
    public IActionResult GetAdministrator()
    {
        return View();
    }
    
    [HttpPost]
    [Route("user")]
    public IActionResult PostUser()
    {
        return View();
    }

    [HttpPost]
    [Route("administrator")]
    public IActionResult PostAdministrator(string login, string password)
    {
        return View();
    }*/
}