using Application.Dtos;
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
        if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(name) ||
            string.IsNullOrEmpty(contact) || !await userInteractor.Registration(new UserRequestDto(login, password,
                name, contact, string.IsNullOrEmpty(defaultAddress) ? null : defaultAddress)))
            return BadRequest();
        
        return Redirect("/login/user");
    }
}