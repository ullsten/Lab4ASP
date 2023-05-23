using Lab4ASP.Areas.Identity.Pages.Account.Manage;
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

        // GET: Avaible options for logged in user when borrow book
        public async Task<IActionResult> LoanBook()
        {
            var currentUser = await _userManager.GetUserAsync(User); //Get loggedInUsers info.
            var currentUserFirstName = currentUser.FirstName;        //Get loggedInUsers firstName

            //filter option so only logged in users name visible in list
            var availableUsers = _userManager.Users
                .Where(u => u.FirstName == currentUserFirstName)
                .ToList();

            //filter avaible books where quantity != 0
            var availableBooks = _context.Books
                .Where(b => b.Quantity != 0)
                .ToList();

            ViewData["FK_UserId"] = new SelectList(availableUsers, "Id", "FullName");
            ViewData["FK_BookId"] = new SelectList(availableBooks, "BookId", "BookTitle");

            return View();
        }

        // POST: LoanHistories/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoanBook([Bind("LoanHistoryId,FK_UserId,FK_BookId")] LoanHistory loanHistory)
        {
            if (ModelState.IsValid)
            {
                // Set loanEnd to 9 days after loan start
                loanHistory.LoanEnd = loanHistory.LoanStart.AddDays(9);
                // Set IsLoaned to true after submitting a new loan
                loanHistory.IsLoaned = true;

                _context.Add(loanHistory);
                await _context.SaveChangesAsync();

                // Decrease the quantity of the borrowed book by 1
                var borrowedBook = await _context.Books.FindAsync(loanHistory.FK_BookId);
                if (borrowedBook != null)
                {
                    borrowedBook.Quantity--;
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(GetLoggedInUserBook));
            }

            ViewData["FK_UserId"] = new SelectList(_userManager.Users, "Id", "FullName", loanHistory.FK_UserId);
            ViewData["FK_BookId"] = new SelectList(_context.Books, "BookId", "BookTitle");

            return View(loanHistory);
        }


        //Show book(s) for logged in user only
        public async Task<ActionResult<IEnumerable<UserBookViewModel>>> GetLoggedInUserBook(string searchString)
        {
            // Get the current logged-in user to filter result 
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return BadRequest();
            }
            // Query the books borrowed by the current user
            var borrowedBook = from l in _context.LoanHistories
                               join u in _userManager.Users on l.FK_UserId equals u.Id
                               join b in _context.Books on l.FK_BookId equals b.BookId
                               where u.Email == currentUser.Email // Filter by current user's ID
                               where l.IsLoaned == true
                               select new UserBookViewModel
                               {
                                   LoanHistoryId = l.LoanHistoryId,
                                   UserName = u.FullName,
                                   BookTitle = b.BookTitle,
                                   BookDescription = b.BookDescription,
                                   LoanStart = l.LoanStart,
                                   LoanEnd = l.LoanEnd,
                                   IsLoaned = l.IsLoaned,
                                   IsReturned = l.IsReturned,
                                   ReturnedDate = l.ReturnedDate,
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
            ViewData["FK_UserId"] = new SelectList(_userManager.Users, "Id", "FullName", loanHistory.FK_UserId);
            ViewData["FK_BookId"] = new SelectList(_context.Books, "BookId", "BookTitle");
            return View(loanHistory);
        }

        // POST: LoanHistories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LoanHistoryId,FK_UserId,FK_BookId,LoanStart,LoanEnd,IsLoaned,IsReturned")] LoanHistory loanHistory)
        {
            if (id != loanHistory.LoanHistoryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Retrieve the original loan history from the database
                    var originalLoanHistory = _context.LoanHistories.AsNoTracking().FirstOrDefault(lh => lh.LoanHistoryId == id);
                    if (originalLoanHistory == null)
                    {
                        return NotFound();
                    }

                    // Preserve the original LoanStart and LoanEnd values
                    loanHistory.LoanStart = originalLoanHistory.LoanStart;
                    loanHistory.LoanEnd = originalLoanHistory.LoanEnd;

                    // Update the loan history
                    if (loanHistory.IsReturned == true)
                    {
                        loanHistory.ReturnedDate = DateTime.Now;

                        // Retrieve the associated book from the database
                        var book = await _context.Books.FindAsync(loanHistory.FK_BookId);
                        if (book != null)
                        {
                            // Increase the quantity of the book
                            book.Quantity += 1;
                            _context.Update(book);
                        }

                        // Set IsLoaned to false when IsReturned is true
                        loanHistory.IsLoaned = false;
                    }

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

            ViewData["FK_UserId"] = new SelectList(_userManager.Users, "Id", "FullName", loanHistory.FK_UserId);
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
