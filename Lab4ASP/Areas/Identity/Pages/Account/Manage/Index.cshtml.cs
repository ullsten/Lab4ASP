// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Lab4ASP.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Lab4ASP.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public IndexModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }



        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Display(Name = "First Name")]
            public string FirstName { get; set; }

            [Display(Name = "Last Name")]
            public string LastName { get; set; }

            [Display(Name = "Username")]
            public string Username { get; set; }

            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }

            [Display(Name = "Profile Picture")]
            public byte[] ProfilePicture { get; set; }

            [Display(Name = "Email Confirmed")]
            public bool EmailConfirmed { get; set; }


            [TempData]
            public string StatusMessage { get; set; }
            [TempData]
            public string UserNameChangeLimitMessage { get; set; }
            [BindProperty]
            public InputModel Input { get; set; }
        }

        private async Task LoadAsync(ApplicationUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            var firstName = user.FirstName;
            var lastName = user.LastName;
            var profilePicture = user.ProfilePicture;
            Username = userName;
            Input = new InputModel
            {
                PhoneNumber = phoneNumber,
                Username = userName,
                FirstName = firstName,
                LastName = lastName,
                ProfilePicture = profilePicture,
                StatusMessage = StatusMessage,
                //UserNameChangeLimitMessage = $"You can change your username {user.UsernameChangeLimit} more time(s).",
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            var firstName = user.FirstName;
            var lastName = user.LastName;
            var profilePicture = user.ProfilePicture;

            Username = userName;

            Input = new InputModel
            {
                PhoneNumber = phoneNumber,
                Username = userName,
                FirstName = firstName,
                LastName = lastName,
                ProfilePicture = profilePicture,
                StatusMessage = StatusMessage,
                UserNameChangeLimitMessage = $"You can change your username {user.UsernameChangeLimit} more time(s)."
            };

            return Page();
        }


        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            // First, update first name and last name if necessary
            var firstName = user.FirstName;
            var lastName = user.LastName;

            if (Input.FirstName != firstName)
            {
                user.FirstName = Input.FirstName;
                StatusMessage = "First name updated";
            }

            if (Input.LastName != lastName)
            {
                user.LastName = Input.LastName;
                if (StatusMessage == null) // check if the status message has already been set
                {
                    StatusMessage = "Last name updated";
                }
                else
                {
                    StatusMessage += " and last name updated"; // if the status message has already been set, concatenate the new message
                }
            }

            // Next, update phone number if necessary
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);

                if(StatusMessage == null)
                {
                    StatusMessage = "Phone number updated";
                }
                else
                {
                    StatusMessage += " and phone number updated";
                }

                if (!setPhoneResult.Succeeded)
                {
                    var userId = await _userManager.GetUserIdAsync(user);
                    throw new InvalidOperationException($"Unexpected error occurred setting phone number for user with ID '{userId}'.");
                }
            }

            //username( not working propertly)
            if (user.UsernameChangeLimit > 0)
            {
                if (Input.Username != user.UserName)
                {
                    var userNameExists = await _userManager.FindByNameAsync(Input.Username);
                    if (userNameExists != null)
                    {
                        StatusMessage = "User name already taken. Select a different username.";
                    }
                    else
                    {
                        var setUserName = await _userManager.SetUserNameAsync(user, Input.Username);
                        if (!setUserName.Succeeded)
                        {
                            StatusMessage = "Unexpected error when trying to set user name.";
                        }
                        else
                        {
                            user.UsernameChangeLimit -= 1;
                            await _userManager.UpdateAsync(user);
                            StatusMessage = "Your username is updated.";
                        }
                    }
                }
            }




            // Finally, update profile picture if necessary
            if (Request.Form.Files.Count > 0)
            {
                IFormFile file = Request.Form.Files.FirstOrDefault();
                if (file != null)
                {
                    using (var dataStream = new MemoryStream())
                    {
                        await file.CopyToAsync(dataStream);
                        user.ProfilePicture = dataStream.ToArray();
                    }
                }
            }

            // Save changes to the user and refresh sign-in
            await _userManager.UpdateAsync(user);
            await _signInManager.RefreshSignInAsync(user);

            return RedirectToPage();
        }


    }
}
