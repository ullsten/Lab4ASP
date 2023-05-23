using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using Lab4ASP.Models.JunctionTables;
using Microsoft.AspNetCore.Identity;

namespace Lab4ASP.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [NotMapped]
        [DisplayName("Customer")]
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
