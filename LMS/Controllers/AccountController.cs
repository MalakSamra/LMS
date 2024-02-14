using LMS.Data;
using LMS.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace LMS.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context = new AppDbContext();
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string returnURL)
        {
            if (ModelState.IsValid)
            {
                var user = _context.users.FirstOrDefault(a => a.User_Name == model.UserName);
                if (user != null)
                {
                    if(user.Password == model.Password)
                    {
                        //Response.Cookies.Append("id", user.Id.ToString());
                        HttpContext.Session.SetInt32("UserId", user.Id);
                        var claims = new List<Claim>
                        {
                            new Claim (ClaimTypes.Name, model.UserName),
                            new Claim (ClaimTypes.Role, user.Role)
                        };
                        ClaimsIdentity CI = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                        foreach (var claim in claims){
                            CI.AddClaim(claim);
                        }
                        ClaimsPrincipal CP = new ClaimsPrincipal(CI);
                        await HttpContext.SignInAsync(CP);
                        if(returnURL != null)
                        {
                            return LocalRedirect(returnURL);
                        }
                        if (user.Role == "student")
                        {
                            return RedirectToAction("Index", "Student");
                        }
                        return RedirectToAction("Index","Home");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Invalid Password");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Invalid Username");
                }
            }
            return View();
        }
        public async Task<IActionResult> LogOut()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                await HttpContext.SignOutAsync();
            }
            return RedirectToAction("Login", "Account");
        }
        
    }
}
