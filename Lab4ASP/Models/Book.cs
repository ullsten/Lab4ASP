using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Lab4ASP.Enums;
using Lab4ASP.Models.JunctionTables;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Lab4ASP.Models
{
    public class Book
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Range(1000, int.MaxValue)] //start value to 1000
        [DisplayName("Id")]
        public int BookId { get; set; }

        [Required]
        [StringLength(50)]
        [DisplayName("Title")]
        public string BookTitle { get; set; }

        [Required]
        [StringLength(250)]
        [DisplayName("Description")]
        public string BookDescription { get; set; }

        [Required]
        [DisplayName("Published")]
        public int PublishedYear { get; set; } //change to datetime

        public int Quantity { get; set; } = 2;

        [Required]
        [ForeignKey("BookTypes")]
        public int FK_BookTypeId { get; set; }
        public BookType? BookTypes { get; set; }

        public ICollection<BookAuthor>? BooksAuthors { get; set; }
        public ICollection<LoanHistory>? LoanHistories { get; set; }
    }
}
