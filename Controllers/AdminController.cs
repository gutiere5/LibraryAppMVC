namespace LibraryAppMVC.Controllers
{
    using LibraryAppMVC.Data;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize(AuthenticationSchemes = "LibraryApp", Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly LibraryContext _context;

        public AdminController(LibraryContext context)
        {
            _context = context;
        }

        // GET: Admin/Dashboard
        public IActionResult AdminDashboard()
        {
            return View();
        }
    }
}
