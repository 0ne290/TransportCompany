using System.Security.Claims;
using Application.Interactors;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
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
    public async Task<IActionResult> PostUser(string login, string password, string remember)// Тогда действия входа должны возвращать 200 или 400. Если 200, то JavaScript-код на клиенте сам выполнит переадресацию. Если 400 - встроит в документ сообщение о неправильном вводе
    {
        if (!await userInteractor.Login(login, password))
            return BadRequest();
        
        var claims = new[] { new Claim(ClaimTypes.Name, login), new Claim(ClaimTypes.Role, "User") };
        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

        AuthenticationProperties? authProperties = null;
        if (remember == "yes")
            authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                IsPersistent = true
            };
        else if (remember != "no")
            
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal, authProperties);
            
        return Redirect("/user");
    }
    
    [HttpPost]
    [Route("administrator")]
    public async Task<IActionResult> PostAdministrator(string login, string password, string remember)
    {
        if (login != "nimda" || password != "4202drowssap")
            return BadRequest();
        
        var claims = new[] { new Claim(ClaimTypes.Role, "Administrator") };
        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);
            
        return Redirect("/administrator");
    }
}