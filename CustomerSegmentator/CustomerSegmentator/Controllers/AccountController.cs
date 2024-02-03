using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CustomerSegmentator.Controllers;

public class AccountController : Controller
{
    private readonly IOptions<AppOptions> _appOptions;

    public AccountController(IOptions<AppOptions> appOptions)
    {
        _appOptions = appOptions;
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View(new LoginViewModel());
    }

    [HttpPost]
    public IActionResult Login(LoginViewModel model)
    {

        var user = _appOptions.Value.Users.FirstOrDefault(u => u.Login == model.Username && u.Password == model.Password);

        if (user != null)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Role, user.Role)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                // Customize any additional properties
            };

            HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);



            return user.Role=="Administrator"?
                RedirectToAction("Create", "CustomerArrivedEvent"):
                RedirectToAction("Index", "CustomerArrivedEvent");
        }

        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
        return View(model);
    }

    public IActionResult Logout()
    {
        HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login", "Account");
    }
}
public class LoginViewModel
{
    public string Username { get; set; }
    public string Password { get; set; }
}