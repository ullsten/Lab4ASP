using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNet.Identity;

namespace Lab4ASP.Models.ViewModels
{
    public class UserBookViewModel
    {
        public int LoanHistoryId { get; set; }

        [DisplayName("Borrower")]
        public string? UserName { get; set; }

        [DisplayName("Book Title")]
        public string? BookTitle { get; set; }

        [DisplayName("Book Title: ")]
        public string? RndTitle { get; set; }

        [DisplayName("Description")]
        public string? BookDescription { get; set; }

        [DisplayName("Description: ")]
        public string? RndDescription { get; set; }

        [DisplayName("Loaned")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? LoanStart { get; set; }

        [DisplayName("Last loan day")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? LoanEnd { get; set; } = DateTime.Now;

        public bool? IsLoaned { get; set; }
        public bool? IsReturned { get; set; }
        public DateTime ReturnedDate { get; set; }

        public string Borrower { get; set; }

        //Relation for borrow book
        [DisplayName("User")]
        public int FK_UserId { get; set; }
        public Users? Users { get; set; }

        [DisplayName("Book")]
        public int FK_BookId { get; set; }
        public Book? Books { get; set; }
    }
}
