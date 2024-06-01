using System.Security.Claims;
using Application.Interactors;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

[Route("login")]
public class LoginController(UserInteractor userInteractor, ILogger logger) : Controller
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
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(2),
                IsPersistent = true
            };
        else if (remember != "no")
            logger.LogWarning("\"{remember}\" value of the \"remember\" parameter is invalid", remember);
            
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal, authProperties);
            
        return Redirect("/user");
    }
    
    [HttpPost]
    [Route("administrator")]
    public async Task<IActionResult> PostAdministrator(string login, string password, string remember)// В идеале remember должен быть bool, а все входные данные действий должны быть ViewModel. Тогда не придется каждый раз валидировать формат входных данных и они будут стандартизированы между фронтом и бэком, т. к. ASP.NET сам будет следить, чтобы форматы на фронте и на бэке совпадали - минимизация кода и вероятности облажаться и проще вносить изменения
    {
        if (login != "nimda" || password != "4202drowssap")
            return BadRequest();
        
        var claims = new[] { new Claim(ClaimTypes.Role, "Administrator") };
        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

        AuthenticationProperties? authProperties = null;
        if (remember == "yes")
            authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(2),
                IsPersistent = true
            };
        else if (remember != "no")
            logger.LogWarning("\"{remember}\" value of the \"remember\" parameter is invalid", remember);
            
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal, authProperties);
            
        return Redirect("/administrator");
    }
}
