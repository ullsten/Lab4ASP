using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Lab4ASP.Data;
using Lab4ASP.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net.Mail;
using System.Text.Encodings.Web;
using NuGet.Versioning;

namespace Lab4ASP.Controllers
{
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private IEmailSender _emailSender;

        public UsersController(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailSender emailSender)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
        }

        public async Task<IActionResult> Index(string userAddedId)
        {
            // Check if userAddedId is not null or empty
            if (!string.IsNullOrEmpty(userAddedId))
            {
                TempData["UserAddedId"] = userAddedId;
            }

            var users = await _userManager.Users.ToListAsync();

            if (users != null)
            {
                return View(users);
            }
            else
            {
                return Problem("User collection is null.");
            }
        }

      

        // GET: Customers/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }


        // POST: Customers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("UserId,FirstName,LastName,UserName,PhoneNumber,Email")] ApplicationUser user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingUser = await _userManager.FindByIdAsync(id);
                    if (existingUser == null)
                    {
                        return NotFound();
                    }

                    existingUser.FirstName = user.FirstName;
                    existingUser.LastName = user.LastName;
                    existingUser.PhoneNumber = user.PhoneNumber;
                    existingUser.Email = user.Email;
                    existingUser.UserName = user.UserName;

                    // Update the user using UserManager
                    var result = await _userManager.UpdateAsync(existingUser);

                    if (result.Succeeded)
                    {
                        // Update was successful
                        return RedirectToAction(nameof(Index));
                    }

                    // If update failed, add model errors to ModelState
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(user.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return View(user);
        }

        // GET: Customers/Create
        public IActionResult Create()
        {
            return View();
        }

        //Add new User to Identity table, set username from email, password being hashed.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,FirstName,LastName,PhoneNumber,Email,PasswordHash")] ApplicationUser user)
        {
            //if (ModelState.IsValid)
            //{
                // Set the username based on the email address
                MailAddress address = new MailAddress(user.Email);
                string userName = address.User;
                user.UserName = userName;

                // Set the password for the user
                user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, user.PasswordHash);
       

                // Create the new user
                var result = await _userManager.CreateAsync(user);

                if (result.Succeeded)
                {
                    // new user get Basic as role
                    await _userManager.AddToRoleAsync(user, Enums.Roles.Basic.ToString());

                    // Send confirmation email
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = Url.Action(
                        "ConfirmEmail",
                        "Account",
                        new { userId = user.Id, code },
                        protocol: HttpContext.Request.Scheme);

                    await _emailSender.SendEmailAsync(user.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    // Store the newly created user ID in session
                    HttpContext.Session.SetString("NewlyCreatedUserId", user.Id);

                    // Show success message
                    TempData["SuccessMessage"] = "User successfully added.";
                    //highlight new user
                    TempData["UserAdded"] = true;
                    TempData["UserAddedId"] = user.Id;

                    return RedirectToAction(nameof(Index), new {userAddedId = user.Id});
                }


                // If creation failed, add model errors to ModelState
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            //}
            // If ModelState is not valid, return the user to the view
            return View(user);
        }



        // GET: Customers/Details/5
        public async Task<IActionResult> Details(string? id)
        {
            if (id == null || _userManager.Users == null)
            {
                return NotFound();
            }

            var user = await _userManager.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

       

        // GET: Customers/Delete/5
        public async Task<IActionResult> Delete(string? id)
        {
            if (id == null || _userManager.Users == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_userManager.Users == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Users'  is null.");
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                var result = await _userManager.DeleteAsync(user);
                if (!result.Succeeded)
                {
                    // Handle delete failure if needed
                    return Problem("Failed to delete user.");
                }
            }

            return RedirectToAction(nameof(Index));
        }


        private bool CustomerExists(string id)
        {
          return (_userManager.Users?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
