using Lab4ASP.Data;
using Lab4ASP.Models;
using Lab4ASP.Models.JunctionTables;
using Lab4ASP.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Lab4ASP.Controllers
{
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    public class UserMenuController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager; //Get data from logged in user if needed

        public UserMenuController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        // GET: LoanHistories/Create
        public IActionResult LoanBook()
        {
            var userIdsOfLoanedBooks = _context.LoanHistories.Select(l => l.FK_UserId).Distinct().ToList();

            var availableBooks = _context.Books.Where(b => !userIdsOfLoanedBooks.Contains(b.BookId)).ToList();

            ViewData["FK_UserId"] = new SelectList(_context.Users, "UserId", "FullName");
            ViewData["FK_BookId"] = new SelectList(availableBooks, "BookId", "BookTitle");

            return View();
        }


        // POST: LoanHistories/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoanBook([Bind("LoanHistoryId,FK_UserId,FK_BookId,LoanStart")] LoanHistory loanHistory)
        {
            if (ModelState.IsValid)
            {
                //Set loanEnd to 9 days after loan start
                loanHistory.LoanEnd = loanHistory.LoanStart.AddDays(9);
                //Set IsLoaned to true after submit new loan
                loanHistory.IsLoaned = true;

                _context.Add(loanHistory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(GetLoggedInUserBook));
            }
            ViewData["FK_UserId"] = new SelectList(_context.Users, "UserId", "Email", loanHistory.FK_UserId);
            return View(loanHistory);
        }

        public async Task<ActionResult<IEnumerable<UserBookViewModel>>> GetLoggedInUserBook(string searchString)
        {
            // Get the current logged-in user
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return BadRequest();
            }
            // Query the books borrowed by the current user
            var borrowedBook = from l in _context.LoanHistories
                               join u in _context.Users on l.FK_UserId equals u.UserId
                               join b in _context.Books on l.FK_BookId equals b.BookId
                               where u.Email == currentUser.Email // Filter by current user's ID
                               select new UserBookViewModel
                               {
                                   LoanHistoryId = l.LoanHistoryId,
                                   UserName = u.FullName,
                                   BookTitle = b.BookTitle,
                                   BookDescription = b.BookDescription,
                                   LoanStart = l.LoanStart,
                                   LoanEnd = l.LoanEnd,
                               };

            var borrowedBookResult = await borrowedBook.AsNoTracking().ToListAsync();

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                borrowedBookResult = borrowedBookResult
                    .Where(u => u.UserName.StartsWith(searchString, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }
            return View(borrowedBookResult);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.LoanHistories == null)
            {
                return NotFound();
            }

            var loanHistory = await _context.LoanHistories.FindAsync(id);
            if (loanHistory == null)
            {
                return NotFound();
            }

            //Need to show value in dropdown in view
            ViewData["FK_UserId"] = new SelectList(_context.Users, "UserId", "FullName", loanHistory.FK_UserId);
            ViewData["FK_BookId"] = new SelectList(_context.Books, "BookId", "BookTitle");
            return View(loanHistory);
        }

        // POST: LoanHistories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LoanHistoryId,FK_UserId,FK_BookId,LoanStart,LoanEnd,IsLoaned")] LoanHistory loanHistory)
        {
            if (id != loanHistory.LoanHistoryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Calculate the new LoanEnd value
                    loanHistory.LoanEnd = loanHistory.LoanStart.AddDays(9);

                    _context.Update(loanHistory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LoanHistoryExists(loanHistory.LoanHistoryId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(GetLoggedInUserBook));
            }
            ViewData["FK_UserId"] = new SelectList(_context.Users, "UserId", "Email", loanHistory.FK_UserId);
            return View(loanHistory);
        }


        // GET: LoanHistories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.LoanHistories == null)
            {
                return NotFound();
            }

            var loanHistory = await _context.LoanHistories
                .Include(l => l.Users)
                .FirstOrDefaultAsync(m => m.LoanHistoryId == id);
            if (loanHistory == null)
            {
                return NotFound();
            }

            return View(loanHistory);
        }

        // POST: LoanHistories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.LoanHistories == null)
            {
                return Problem("Entity set 'ApplicationDbContext.LoanHistories'  is null.");
            }
            var loanHistory = await _context.LoanHistories.FindAsync(id);
            if (loanHistory != null)
            {
                _context.LoanHistories.Remove(loanHistory);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(GetLoggedInUserBook));
        }

        private bool LoanHistoryExists(int id)
        {
            return (_context.LoanHistories?.Any(e => e.LoanHistoryId == id)).GetValueOrDefault();
        }
    }
}
