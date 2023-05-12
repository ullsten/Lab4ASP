﻿using Microsoft.AspNetCore.Identity;

namespace Lab4ASP.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int UsernameChangeLimit { get; set; }
        public byte[]? ProfilePicture { get; set; }
    }
}
