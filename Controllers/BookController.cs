namespace LibraryAppMVC.Controllers
{
    using LibraryAppMVC.Data;
    using LibraryAppMVC.Models;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;

    public class BookController : Controller
    {
        private readonly LibraryContext _context;

        public BookController(LibraryContext context)
        {
            _context = context;
        }

        // GET: Book
        public async Task<IActionResult> Index()
        {
            var libraryContext = _context
                .Books.Include(b => b.Author)
                .Include(b => b.Borrower)
                .Include(b => b.Publisher);
            return View(await libraryContext.ToListAsync());
        }

        // GET: Book/Create
        public IActionResult Create()
        {
            ViewData["AuthorId"] = new SelectList(_context.Authors, "AuthorId", "FullName");
            ViewData["BorrowerId"] = new SelectList(_context.Users, "UserId", "Password");
            ViewData["PublisherId"] = new SelectList(_context.Publishers, "PublisherId", "Name");
            return View();
        }

        // POST: Book/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("BookId,Title,ISBN,AuthorId,PublisherId,BorrowerId")] Book book
        )
        {
            if (ModelState.IsValid)
            {
                _context.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AuthorId"] = new SelectList(
                _context.Authors,
                "AuthorId",
                "FullName",
                book.AuthorId
            );
            ViewData["BorrowerId"] = new SelectList(
                _context.Users,
                "UserId",
                "Password",
                book.BorrowerId
            );
            ViewData["PublisherId"] = new SelectList(
                _context.Publishers,
                "PublisherId",
                "Name",
                book.PublisherId
            );
            return View(book);
        }

        // GET: Book/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            ViewData["AuthorId"] = new SelectList(
                _context.Authors,
                "AuthorId",
                "FullName",
                book.AuthorId
            );
            ViewData["BorrowerId"] = new SelectList(
                _context.Users,
                "UserId",
                "Password",
                book.BorrowerId
            );
            ViewData["PublisherId"] = new SelectList(
                _context.Publishers,
                "PublisherId",
                "Name",
                book.PublisherId
            );
            return View(book);
        }

        // POST: Book/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("BookId,Title,ISBN,AuthorId,PublisherId,BorrowerId")] Book book
        )
        {
            if (id != book.BookId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(book);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.BookId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["AuthorId"] = new SelectList(
                _context.Authors,
                "AuthorId",
                "FullName",
                book.AuthorId
            );
            ViewData["BorrowerId"] = new SelectList(
                _context.Users,
                "UserId",
                "Password",
                book.BorrowerId
            );
            ViewData["PublisherId"] = new SelectList(
                _context.Publishers,
                "PublisherId",
                "Name",
                book.PublisherId
            );
            return View(book);
        }

        // GET: Book/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context
                .Books.Include(b => b.Author)
                .Include(b => b.Borrower)
                .Include(b => b.Publisher)
                .FirstOrDefaultAsync(m => m.BookId == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Book/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book != null)
            {
                _context.Books.Remove(book);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(int id)
        {
            return _context.Books.Any(e => e.BookId == id);
        }

        // GET: Books/Search
        public async Task<IActionResult> Search(string searchString)
        {
            var books = from b in _context.Books select b;

            if (!string.IsNullOrEmpty(searchString))
            {
                books = books.Where(b =>
                    b.Title.Contains(searchString) || b.ISBN.Contains(searchString)
                );
            }

            return View(await books.ToListAsync());
        }

        // GET: Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context
                .Books.Include(b => b.Author)
                .Include(b => b.Publisher)
                .FirstOrDefaultAsync(m => m.BookId == id);

            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // GET: Books/Borrow/5
        public async Task<IActionResult> Borrow(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books.FindAsync(id);

            if (book == null || book.BorrowerId != null) // Check if already borrowed
            {
                ViewBag.Message = "This book is currently unavailable.";
                return View("Error");
            }

            // Get current user
            var userId = HttpContext.Session.GetString("UserId");
            var user = await _context.Users.FindAsync(int.Parse(userId));

            if (user == null)
            {
                ViewBag.Message = "User not found.";
                return View("Error");
            }

            // Borrow the book
            book.BorrowerId = user.UserId;
            _context.Update(book);
            await _context.SaveChangesAsync();

            return RedirectToAction("Dashboard", "User");
        }
    }
}
