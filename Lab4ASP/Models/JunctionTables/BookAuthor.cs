using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lab4ASP.Models.JunctionTables
{
    public class BookAuthor
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Range(1, int.MaxValue)]
        public int BookAuthorId { get; set; }

        [Required]
        [ForeignKey("Books")]
        public int FK_BookId { get; set; }
        public Book? Books { get; set; }

        [Required]
        [ForeignKey("Authors")]
        public int AuthorId { get; set; }
        public Author? Authors { get; set; }

        public ICollection<LoanHistory>? LoanHistories { get; set; }
    }
}