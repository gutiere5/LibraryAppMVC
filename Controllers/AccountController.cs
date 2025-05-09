namespace LibraryAppMVC.Models
{
    using System.Security.Claims;
    using LibraryAppMVC.Data;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    public class AccountController : Controller
    {
        private readonly LibraryContext _context;

        public AccountController(LibraryContext context)
        {
            _context = context;
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        // GET: Account/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string username, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u =>
                u.UserName == username && u.Password == password
            );

            if (user == null)
            {
                TempData["ErrorMessage"] = "Invalid username or password.";
                return View();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
            };

            var identify = new ClaimsIdentity(claims, "LibraryApp");
            var principal = new ClaimsPrincipal(identify);

            await HttpContext.SignInAsync("LibraryApp", principal);

            if (user.Role == "Admin")
            {
                return RedirectToAction("AdminDashboard", "Admin");
            }
            return RedirectToAction("UserDashboard", "User");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("LibraryApp");
            return RedirectToAction("Login", "Account");
        }
    }
}
