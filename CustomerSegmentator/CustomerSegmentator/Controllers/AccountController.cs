using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
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
        var user = _appOptions.Value.Users.FirstOrDefault(u => u.Login == model.Username &&
                                                               u.PasswordHash == HashPassword( model.Password,_appOptions.Value.Salt));

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
                RedirectToAction("Administrator", "CustomerArrivedEvent"):
                RedirectToAction("Director", "CustomerArrivedEvent");
        }

        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
        return View(model);
    }

    public IActionResult Logout()
    {
        HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login", "Account");
    }

    private static string HashPassword(string password, string salt)
    {
        using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(salt)))
        {
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            byte[] hashBytes = hmac.ComputeHash(passwordBytes);
            return Convert.ToBase64String(hashBytes);
        }
    }
}
public class LoginViewModel
{
    public string Username { get; set; }
    public string Password { get; set; }
}