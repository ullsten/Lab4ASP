using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lab4ASP.Models
{
    public class Book
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BookId { get; set; }

        [Required]
        [StringLength(50)]
        [DisplayName("Book title")]
        public string BookTitle { get; set; }

        [Required]
        [StringLength(50)]
        [DisplayName("Description")]
        public string BookDescription { get; set; }
    }
}
