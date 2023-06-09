﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Lab4ASP.Data;
using Lab4ASP.Models;
using Lab4ASP.Models.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace Lab4ASP.Controllers
{
    public class BooksController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager; //Get data from logged in user if needed

        public BooksController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Books
        //public async Task<IActionResult> Index()
        //{
        //    var applicationDbContext = _context.Books
        //        .Include(b => b.BookTypes)
        //        .Include(b => b.BooksAuthors);
        //    return View(await applicationDbContext.ToListAsync());
        //}

        public async Task<ActionResult<IEnumerable<BookRegisterViewModel>>> Index()
        {
            var bookRegister = await _context.BooksAuthors
                .Include(l => l.Books)
                .Include(l => l.Books.BookTypes)
                .Select(l => new BookRegisterViewModel
                {
                    BookId = l.FK_BookId,
                    BookTitle = l.Books.BookTitle,
                    BookDescription = l.Books.BookDescription,
                    Published = l.Books.PublishedYear,
                    Genre = l.Books.BookTypes.BookTypeName,
                    Author = string.IsNullOrEmpty(l.Authors.AuthorName) ? "No Author" : l.Authors.AuthorName, // Check if author is null or empty!!! Not working fixx later
                    Quantity = l.Books.Quantity,
                    BookPicture = l.Books.BookPicture,
                })
                .ToListAsync();

            return View(bookRegister);
        }



        // GET: Books/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Books == null)
            {
                return NotFound();
            }

            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            ViewData["FK_BookTypeId"] = new SelectList(_context.BookTypes, "BookTypeId", "BookTypeName", book.FK_BookTypeId);
            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BookId,BookTitle,BookDescription,PublishedYear,Quantity,FK_BookTypeId")] Book book, IFormFile bookPicture)
        {
            if (id != book.BookId)
            {
                return NotFound();
            }

            //if (ModelState.IsValid)
            //{
                try
                {
                    if (bookPicture != null && bookPicture.Length > 0)
                    {
                        using (var ms = new MemoryStream())
                        {
                            await bookPicture.CopyToAsync(ms);
                            book.BookPicture = ms.ToArray();
                        }
                    }

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
            //}

            // If the model state is invalid, you can handle the error or return the view with the invalid model.
            ViewData["FK_BookTypeId"] = new SelectList(_context.BookTypes, "BookTypeId", "BookTypeName", book.FK_BookTypeId);
            return View(book);
        }

        // GET: Books/Create
        public IActionResult Create()
        {
            ViewData["FK_BookTypeId"] = new SelectList(_context.BookTypes, "BookTypeId", "BookTypeName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BookId,BookTitle,BookDescription,PublishedYear,Quantity,FK_BookTypeId")] Book book, IFormFile bookPicture)
        {
            //if (ModelState.IsValid)
            //{
                if (bookPicture != null && bookPicture.Length > 0)
                {
                    using (var ms = new MemoryStream())
                    {
                        await bookPicture.CopyToAsync(ms);
                        book.BookPicture = ms.ToArray();
                    }
                }

                _context.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            //}

            //ViewData["FK_BookTypeId"] = new SelectList(_context.BookTypes, "BookTypeId", "BookTypeName", book.FK_BookTypeId);
            return View(book);
        }



        // GET: Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Books == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .Include(b => b.BookTypes)
                .FirstOrDefaultAsync(m => m.BookId == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // GET: Books/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Books == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .Include(b => b.BookTypes)
                .FirstOrDefaultAsync(m => m.BookId == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Books == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Books'  is null.");
            }
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
          return (_context.Books?.Any(e => e.BookId == id)).GetValueOrDefault();
        }

        //**************************************************************

        public async Task<IActionResult> GetRandomBook()
        {
            var randomBook = await _context.Books
                .Include(b => b.BookTypes)          //Hämtar böcker från databasen
                .OrderBy(x => Guid.NewGuid())       //ordnar böckerna random
                .Select(b => new UserBookViewModel      // skapar UserBookViewModel object för varje bok med title+description angivet nedanför
                {
                    RndTitle = b.BookTitle,
                    RndDescription = b.BookDescription
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

        //Could add to view search field
   


    }
}
