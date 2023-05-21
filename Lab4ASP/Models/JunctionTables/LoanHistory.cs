using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.Build.Framework;
using RequiredAttribute = System.ComponentModel.DataAnnotations.RequiredAttribute;

namespace Lab4ASP.Models.JunctionTables
{
    public class LoanHistory
    {
        public int LoanHistoryId { get; set; }

        [ForeignKey("Users")]
        public int FK_UserId { get; set; }
        public Users? Users { get; set; }

        [ForeignKey("Book")]
        public int FK_BookId { get; set; }
        public Book? Books { get; set; }

        [Required]
        [DisplayName("Loan start")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime LoanStart { get; set; } = DateTime.Now;

        [Required]
        [DisplayName("Loan end")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime LoanEnd { get; set; }

        [Required]
        [DisplayName("Borrowed")]
        public bool IsLoaned { get; set; }

        [DisplayName("Returned")]
        public bool IsReturned { get; set; } = false;

        public DateTime ReturnedDate { get; set; }

        //public void MarkAsReturned()
        //{ 
        //    IsLoaned = false;
        //    IsReturned = true; 
        //    ReturnedDate = DateTime.Now;
        //}
    }
}