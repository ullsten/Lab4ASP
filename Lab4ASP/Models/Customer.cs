using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lab4ASP.Models
{
    public class Customer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CustomerId { get; set; }

        [Required]
        [StringLength(30)]
        [DisplayName("First name")]
        public string FirstName { get; set; }
        
        [Required]
        [StringLength(30)]
        [DisplayName("Last name")]
        public string LastName { get; set; }
    }
}
