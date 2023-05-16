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
        [ForeignKey("Customers")]
        public int? FK_CustomerId { get; set; } = null;

        [DisplayName("Customer")]
        public Customer? Customers { get; set; }
    }
}