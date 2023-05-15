using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Lab4ASP.Models.JunctionTables;

namespace Lab4ASP.Models
{
    public class Author
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Range(100, int.MaxValue)]
        public int AuthorId { get; set; }

        [Required]
        [StringLength(50)]
        [DisplayName("Author")]
        public string AuthorName { get; set; }

        public  ICollection<BookAuthor>? BookAuthors { get; set; }    
    }
}
