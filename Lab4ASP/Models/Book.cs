using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Lab4ASP.Enums;
using Lab4ASP.Models.JunctionTables;

namespace Lab4ASP.Models
{
    public class Book
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Range(1000, int.MaxValue)] //start value to 1000
        public int BookId { get; set; }

        [Required]
        [StringLength(50)]
        [DisplayName("Book title")]
        public string BookTitle { get; set; }

        [Required]
        [StringLength(250)]
        [DisplayName("Description")]
        public string BookDescription { get; set; }

        [Required]
        [StringLength(12)]
        [DisplayName("Published")]
        public int PublishedYear { get; set; }

        [Required]
        public BookTypes BookTypes { get; set; }
        public ICollection<BookAuthor>? BookAuthors { get; set; }
        public ICollection<LoanHistory>? LoanHistories { get; set; }
    }
}
