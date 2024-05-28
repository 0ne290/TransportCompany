using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

[Route("user")]
public class UserController : Controller
{
    //public UserController(ILogger<LoginController> logger)
    //{
    //    _logger = logger;
    //}
    
    [HttpGet]
    [Route("user")]
    public IActionResult GetUser()
    {
        return View();
    }

    [HttpGet]
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
    }
}