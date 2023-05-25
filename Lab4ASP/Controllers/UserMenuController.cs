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

        //Logged in users dashboard shows only current users loans
        public async Task<ActionResult<IEnumerable<UserBookViewModel>>> GetLoggedInUserBook(string searchString, bool? switchReturned, bool? switchLoaned)
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
                               where u.Id == currentUser.Id // Filter by current user's id to show realated loans
                               orderby l.LoanStart descending
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
                                   DaysLeft = l.DaysLeft,
                                   BookPicture = l.Books.BookPicture,
                               };

            if (switchReturned == true) // Only returned books
            {
                borrowedBook = borrowedBook.Where(l => l.IsReturned);
            }
            if (switchLoaned == true)
            {
                borrowedBook = borrowedBook.Where(l => l.IsLoaned);
            }

            var borrowedBookResult = await borrowedBook.AsNoTracking().ToListAsync();

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                borrowedBookResult = borrowedBookResult
                    .Where(u => u.BookTitle.StartsWith(searchString, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            ViewBag.SwitchReturned = switchReturned; // Store the current switch state returned
            ViewBag.SwitchLoaned = switchLoaned;    //Store the current switch state loaned

            return View(borrowedBookResult);
        }

        public async Task<IActionResult> Create()
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

            ViewData["FK_UserId"] = new SelectList(availableUsers, "Id", "FullName"); // Display the user's full name in the dropdown
            ViewData["FK_BookId"] = new SelectList(availableBooks, "BookId", "BookTitle"); // Display the book title in the dropdown

            return View();
        }

        // POST: LoanHistories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LoanHistoryId,FK_UserId,FK_BookId,LoanStart")] LoanHistory loanHistory)
        {
            if (ModelState.IsValid)
            {
                //Set loanEnd to 9 days after loan start
                //loanHistory.LoanEnd = loanHistory.LoanStart.AddDays(9);
                //Set IsLoaned to true after submit new loan
                loanHistory.IsLoaned = true;

                _context.Add(loanHistory);
                await _context.SaveChangesAsync();

                // Decrease the quantity of the borrowed book by 1 - if 0 book not shows in droppdown list
                var borrowedBook = await _context.Books.FindAsync(loanHistory.FK_BookId);
                if (borrowedBook != null)
                {
                    borrowedBook.Quantity--;
                    await _context.SaveChangesAsync();
                }

                // Get the latest loan history entry
                var latestLoan = await _context.LoanHistories.OrderByDescending(l => l.LoanHistoryId).FirstOrDefaultAsync();

                // Pass the latest loan history entry ID to the view
                TempData["LatestLoanId"] = latestLoan?.LoanHistoryId;
                TempData["LoanCreatedMessage"] = "Loan application created successfully.";

                return RedirectToAction(nameof(GetLoggedInUserBook));
            }
            ViewData["FK_UserId"] = new SelectList(_userManager.Users, "Id", "FullName", loanHistory.FK_UserId);
            return View(loanHistory);
        }

        // GET: LoanHistories/Edit/5
        public async Task<IActionResult> Edit(int id)
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

            var currentUser = await _userManager.GetUserAsync(User); // Get loggedInUser's info.
            var currentUserFirstName = currentUser.FirstName; // Get loggedInUser's firstName

            // Filter option so only loggedInUser's name is visible in the list
            var availableUsers = _userManager.Users
                .Where(u => u.FirstName == currentUserFirstName)
                .ToList();

            // Filter available books where quantity > 0 or the book is the one being edited
            var availableBooks = _context.Books
                .Where(b => b.Quantity > 0 || b.BookId == loanHistory.FK_BookId)
                .ToList();

            // Need to show values in dropdown in the view
            ViewData["FK_UserId"] = new SelectList(availableUsers, "Id", "FullName", loanHistory.FK_UserId);
            ViewData["FK_BookId"] = new SelectList(availableBooks, "BookId", "BookTitle");

            return View(loanHistory);
        }

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
                if (loanHistory.IsReturned)
                {
                    loanHistory.ReturnedDate = DateTime.Now;
                    loanHistory.IsLoaned = false;

                    // Increase quantity for returned book
                    var borrowedBook = await _context.Books.FindAsync(loanHistory.FK_BookId);
                    if (borrowedBook != null)
                    {
                        borrowedBook.Quantity++;
                        await _context.SaveChangesAsync();
                    }
                }
                else if (loanHistory.IsLoaned)
                {
                    // Degrease quantity for loaned book
                    var borrowedBook = await _context.Books.FindAsync(loanHistory.FK_BookId);
                    if (borrowedBook != null)
                    {
                        borrowedBook.Quantity--;
                        await _context.SaveChangesAsync();
                    }
                }

                try
                {
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

            ViewData["FK_UserId"] = new SelectList(_userManager.Users, "Id", "Email", loanHistory.FK_UserId);
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
