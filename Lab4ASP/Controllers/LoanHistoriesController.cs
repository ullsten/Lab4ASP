using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Lab4ASP.Data;
using Lab4ASP.Models.JunctionTables;
using Lab4ASP.Models.ViewModels;
using Lab4ASP.Models;
using Microsoft.AspNetCore.Identity;

namespace Lab4ASP.Controllers
{
    public class LoanHistoriesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager; //Get logged in user properties

        public LoanHistoriesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: LoanHistories
        public async Task<ActionResult<List<LoanHistoryViewModel>>> Index(string searchString, bool? showLoanedBooks)
        {
            var loanHistoryQuery = from l in _context.LoanHistories
                                   join u in _context.Users on l.FK_UserId equals u.UserId
                                   join b in _context.Books on l.FK_BookId equals b.BookId
                                   orderby u.LastName
                                   select new LoanHistoryViewModel
                                   {
                                       FullName = u.FullName,
                                       BookTitle = b.BookTitle,
                                       LoanStart = l.LoanStart,
                                       LoanEnd = l.LoanEnd,
                                       Isloand = l.IsLoaned,
                                       LoanHistoryId = l.LoanHistoryId
                                   };

            if (showLoanedBooks.HasValue)
            {
                bool isLoaned = showLoanedBooks.Value;
                loanHistoryQuery = loanHistoryQuery.Where(l => l.Isloand == isLoaned);
            }

            var loanHistory = await loanHistoryQuery.ToListAsync();

            return View(loanHistory);
        }

        //Get returned book(s)
        public async Task<ActionResult<List<LoanHistoryViewModel>>> GetReturnedBook(string searchString)
        {
            var loanHistory = await (from l in _context.LoanHistories
                                     join u in _context.Users on l.FK_UserId equals u.UserId
                                     join b in _context.Books on l.FK_BookId equals b.BookId
                                     orderby u.LastName
                                     where l.IsLoaned != true //show only returned book(s)
                                     select new LoanHistoryViewModel //fyller viewmodel med data
                                     {
                                         FullName = u.FullName,
                                         BookTitle = b.BookTitle,
                                         LoanStart = l.LoanStart,
                                         LoanEnd = l.LoanEnd,
                                         Isloand = l.IsLoaned,
                                         LoanHistoryId = l.LoanHistoryId
                                     }).ToListAsync();

            return View(loanHistory);
        }
        //Get users and loaned books by search
        public async Task<ActionResult<List<UserBookViewModel>>> GetUserBook(string searchString, bool? switchLoanSearch)
        {
            var query = from l in _context.LoanHistories
                        join u in _context.Users on l.FK_UserId equals u.UserId
                        join b in _context.Books on l.FK_BookId equals b.BookId
                        orderby u.LastName
                        select new UserBookViewModel
                        {
                            LoanHistoryId = l.LoanHistoryId,
                            UserName = u.FullName,
                            BookTitle = b.BookTitle,
                            BookDescription = b.BookDescription,
                            LoanStart = l.LoanStart,
                            LoanEnd = l.LoanEnd,
                            IsLoaned = l.IsLoaned,
                        };

            //if (switchLoanSearch.HasValue)
            //{
            //    query = query.Where(l => l.IsLoaned == switchLoanSearch.Value);
            //}
            if (switchLoanSearch == true) // Only show returned books
            {
                query = query.Where(l => (bool)!l.IsLoaned);
            }

            var borrowedBook = await query.ToListAsync();

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                borrowedBook = borrowedBook.Where(u => u.UserName.StartsWith(searchString, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            ViewBag.SwitchLoanSearch = switchLoanSearch; // Store the current switch state

            return View(borrowedBook);

        }




        // GET: LoanHistories/Details/5
        public async Task<IActionResult> Details(int? id)
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

        // GET: LoanHistories/Create
        public IActionResult Create()
        {
            ViewData["FK_UserId"] = new SelectList(_context.Users, "UserId", "FullName"); //change what to display in dropdown list
            ViewData["FK_BookId"] = new SelectList(_context.Books, "BookId", "BookTitle"); //change what to display in dropdown list
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
                loanHistory.LoanEnd = loanHistory.LoanStart.AddDays(9);
                //Set IsLoaned to true after submit new loan
                loanHistory.IsLoaned = true;

                _context.Add(loanHistory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FK_UserId"] = new SelectList(_context.Users, "UserId", "Email", loanHistory.FK_UserId);
            return View(loanHistory);
        }

        // GET: LoanHistories/Edit/5
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
                return RedirectToAction(nameof(Index));
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
            return RedirectToAction(nameof(Index));
        }

        private bool LoanHistoryExists(int id)
        {
          return (_context.LoanHistories?.Any(e => e.LoanHistoryId == id)).GetValueOrDefault();
        }
    }
}
