using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Lab4ASP.Models.JunctionTables;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Identity;

namespace Lab4ASP.Models
{
    public class Users : IdentityUser
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        public string? IdentityUserId { get; set; }

        [Required]
        [StringLength(20)]
        [DisplayName("First name")]
        public string FirstName { get; set; }
        
        [Required]
        [StringLength(40)]
        [DisplayName("Last name")]
        public string LastName { get; set; }

        [NotMapped]
        [DisplayName("Customer")]
        public string FullName => $"{FirstName} {LastName}";

        [Required]
        [StringLength(15)]
        [DisplayName("Phone number")]
        public string PhoneNumber { get; set; }
        [Required]
        [StringLength(50)]
        public string Email { get; set; }

        [NotMapped]
        public string Borrower { get; set; }

        //relation
        public ICollection<Address>? Addresses { get; set; }
        public ICollection<LoanHistory>? LoanHistories { get; set; }
    }
}
