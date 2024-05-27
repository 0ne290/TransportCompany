using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Web.Models;

namespace Web.Controllers;

[Route("login")]
public class LoginController : Controller
{
    //public LoginController(ILogger<LoginController> logger)
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