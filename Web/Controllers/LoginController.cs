using Application.Interactors;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

[Route("login")]
public class LoginController(UserInteractor userInteractor) : Controller
{
    [HttpGet]
    [Route("user")]
    public IActionResult GetUser()
    {
        return View("User");
    }

    [HttpGet]
    [Route("administrator")]
    public IActionResult GetAdministrator()
    {
        return View("Administrator");
    }
    
    // В идеале часть логики входа должна быть на фронтенде, чтобы в случае ввода неправильных логина и/или пароля происходило встраивание с помощью AJAX в тот же документ на той же странице сообщения о неправильном вводе
    [HttpPost]
    [Route("user")]
    public async Task<IActionResult> PostUser(string login, string password)// Тогда действия входа должны возвращать 200 или 400. Если 200, то JavaScript-код на клиенте сам выполнит переадресацию. Если 400 - встроит в документ сообщение о неправильном вводе
    {
        if (await userInteractor.Login(login, password))
        {
            return Redirect("user");
        }
        return BadRequest();
    }
    
    [HttpPost]
    [Route("administrator")]
    public IActionResult PostAdministrator(string login, string password)
    {
        if (login == "nimda" && password == "4202drowssap")
        {
            return Redirect("administrator");
        }
        return BadRequest();
    }
}