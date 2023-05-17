using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lab4ASP.Models
{
    public class BookType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BookTypeId { get; set; }
        
        [Required]
        [StringLength(15)]
        [DisplayName("Genre")]
        public string BookTypeName { get; set; }
    }
}
