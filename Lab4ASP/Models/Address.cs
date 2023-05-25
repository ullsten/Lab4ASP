using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lab4ASP.Models
{
    public class Address
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AddressId { get; set; }

        [Required]
        [StringLength(50)]
        public string Street { get; set; }
        
        [Required]
        [StringLength(50)]
        public string City { get; set; }

        [Required]
        [StringLength(6)]
        [DisplayName("Postal code")]
        public string PostalCode { get; set; }

        //Relation
        [ForeignKey("Users")]
        public string? FK_UserId { get; set; } = null;

        [DisplayName("User")]
        public ApplicationUser? Users { get; set; }
    }
}