using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Lab4ASP.Data;
using Lab4ASP.Models.JunctionTables;

namespace Lab4ASP.Controllers
{
    public class BookAuthorsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BookAuthorsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: BookAuthors
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.BooksAuthors
                .Include(b => b.Authors)
                .Include(b => b.Books)
                .Include(b=> b.Books.BookTypes);

            return View(await applicationDbContext.ToListAsync());
        }

        // GET: BookAuthors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.BooksAuthors == null)
            {
                return NotFound();
            }

            var bookAuthor = await _context.BooksAuthors
                .Include(b => b.Authors)
                .Include(b => b.Books)
                .FirstOrDefaultAsync(m => m.BookAuthorId == id);
            if (bookAuthor == null)
            {
                return NotFound();
            }

            return View(bookAuthor);
        }

        // GET: BookAuthors/Create
        public IActionResult Create()
        {
            ViewData["AuthorId"] = new SelectList(_context.Authors, "AuthorId", "AuthorName");
            ViewData["FK_BookId"] = new SelectList(_context.Books, "BookId", "BookDescription");
            return View();
        }

        // POST: BookAuthors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BookAuthorId,FK_BookId,AuthorId")] BookAuthor bookAuthor)
        {
            //if (ModelState.IsValid)
            //{
                _context.Add(bookAuthor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            //}
            ViewData["AuthorId"] = new SelectList(_context.Authors, "AuthorId", "AuthorName", bookAuthor.AuthorId);
            ViewData["FK_BookId"] = new SelectList(_context.Books, "BookId", "BookDescription", bookAuthor.FK_BookId);
            return View(bookAuthor);
        }

        // GET: BookAuthors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.BooksAuthors == null)
            {
                return NotFound();
            }

            var bookAuthor = await _context.BooksAuthors.FindAsync(id);
            if (bookAuthor == null)
            {
                return NotFound();
            }
            ViewData["AuthorId"] = new SelectList(_context.Authors, "AuthorId", "AuthorName", bookAuthor.AuthorId);
            ViewData["FK_BookId"] = new SelectList(_context.Books, "BookId", "BookDescription", bookAuthor.FK_BookId);
            return View(bookAuthor);
        }

        // POST: BookAuthors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BookAuthorId,FK_BookId,AuthorId")] BookAuthor bookAuthor)
        {
            if (id != bookAuthor.BookAuthorId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bookAuthor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookAuthorExists(bookAuthor.BookAuthorId))
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
            ViewData["AuthorId"] = new SelectList(_context.Authors, "AuthorId", "AuthorName", bookAuthor.AuthorId);
            ViewData["FK_BookId"] = new SelectList(_context.Books, "BookId", "BookDescription", bookAuthor.FK_BookId);
            return View(bookAuthor);
        }

        // GET: BookAuthors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.BooksAuthors == null)
            {
                return NotFound();
            }

            var bookAuthor = await _context.BooksAuthors
                .Include(b => b.Authors)
                .Include(b => b.Books)
                .FirstOrDefaultAsync(m => m.BookAuthorId == id);
            if (bookAuthor == null)
            {
                return NotFound();
            }

            return View(bookAuthor);
        }

        // POST: BookAuthors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.BooksAuthors == null)
            {
                return Problem("Entity set 'ApplicationDbContext.BooksAuthors'  is null.");
            }
            var bookAuthor = await _context.BooksAuthors.FindAsync(id);
            if (bookAuthor != null)
            {
                _context.BooksAuthors.Remove(bookAuthor);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookAuthorExists(int id)
        {
          return (_context.BooksAuthors?.Any(e => e.BookAuthorId == id)).GetValueOrDefault();
        }
    }
}
