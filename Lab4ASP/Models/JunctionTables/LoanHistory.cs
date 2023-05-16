using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.Build.Framework;

namespace Lab4ASP.Models.JunctionTables
{
    public class LoanHistory
    {
        public int LoanHistoryId { get; set; }

        [ForeignKey("Customers")]
        public int FK_CustomerId { get; set; }
        public Customer? Customers { get; set; }

        [ForeignKey("Book")]
        public int FK_BookId { get; set; }
        public Book? Books { get; set; }

        [Required]
        [DisplayName("Loan start")]
        public DateTime LoanStart { get; set; }

        [Required]
        [DisplayName("Loan end")]
        public DateTime LoanEnd { get; set; }

        [Required]
        [DisplayName("Is loaned")]
        public bool IsLoaned { get; set; }
    }
}