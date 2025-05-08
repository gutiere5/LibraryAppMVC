namespace LibraryAppMVC.Models
{
    using LibraryAppMVC.Data;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    public class AccountController : Controller
    {
        private readonly LibraryContext _context;

        public AccountController(LibraryContext context)
        {
            _context = context;
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

            if (user != null)
            {
                // Store the user info in the session or authentication system
                HttpContext.Session.SetString("UserId", user.UserId.ToString());

                return RedirectToAction("Dashboard", "User");
            }
            {
                ViewBag.ErrorMessage = "Invalid credentials.";
                return View();
            }
        }
    }
}
