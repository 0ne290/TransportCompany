using Application.Interactors;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

[Route("registration/user")]
public class RegistrationController(UserInteractor userInteractor) : Controller
{
    [HttpGet]
    public IActionResult GetUser()
    {
        return View("User");
    }
    
    [HttpPost]
    public async Task<IActionResult> PostUser(string login, string password, string name, string contact, string defaultAddress)
    {
        if (!await userInteractor.Registration(login, password, name, contact, defaultAddress))
            return BadRequest();
        
        return Redirect("/login/user");
    }
}