using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using Lab4ASP.Models.JunctionTables;
using Microsoft.AspNetCore.Identity;

namespace Lab4ASP.Models
{
    public class ApplicationUser : IdentityUser
    {
        [DisplayName("First Name")]
        public string FirstName { get; set; }
        [DisplayName("Last Name")]
        public string LastName { get; set; }

        //[DisplayName("Email Confirmed")]
        //public bool EmailConfirmed { get; set; }

        [NotMapped]
        [DisplayName("Phone")]
        public string PhoneTitle { get; set; }

        [NotMapped]
        [DisplayName("User")]
        public string FullName => $"{FirstName} {LastName}";
        public int UsernameChangeLimit { get; set; } = 10;
        public byte[]? ProfilePicture { get; set; }

        [NotMapped]
        public string Borrower { get; set; }

        //relation
        public ICollection<Address>? Addresses { get; set; }
        public ICollection<LoanHistory>? LoanHistories { get; set; }
    }
}
