using Lab4ASP.Data;
using Lab4ASP.Models;
using Lab4ASP.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lab4ASP.Controllers
{
    public class DashBoardsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager; //Get logged in user properties

        public DashBoardsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "SuperAdmin")]
        //Get users and loaned books by search
        public async Task<ActionResult<List<UserBookViewModel>>> GetUserBook(string searchString, bool? switchLoanSearch)
        {
            var query = from l in _context.LoanHistories
                        join u in _userManager.Users on l.FK_UserId equals u.Id
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
                            FK_UserId = l.FK_UserId,
                            FK_BookId = l.FK_BookId,
                        };

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

            // Retrieve the list of users and books from the database
            ViewBag.Users = await _userManager.Users.ToListAsync();
            ViewBag.Books = await _context.Books.ToListAsync();

            return View(borrowedBook);

        }

        [Authorize(Roles = "Basic, SuperAdmin")]
        //For user
        public async Task<IActionResult> GetRandomBook()
        {
            var randomBook = await _context.Books
                .Include(b => b.BookTypes)  //Hämtar böcker från databasen
                .OrderBy(x => Guid.NewGuid())
                //ordnar böckerna random
                .Select(b => new UserBookViewModel      // skapar UserBookViewModel object för varje bok med title+description angivet nedanför
                {
                    RndTitle = b.BookTitle,
                    RndDescription = b.BookDescription,
                    BookPicture = b.BookPicture,
                   
                })
                .FirstOrDefaultAsync();         //hämtar första boken i listan som bli den random bok som ska visas.

            if (randomBook == null)
            {
                return NotFound();
            }
            //Skapar en ny lista med bara en bok för att i view visa random books vid reload
            var randomBookCollection = new List<UserBookViewModel> { randomBook };

            return View(randomBookCollection);
        }
    }
}
