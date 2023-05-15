using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

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

        [DisplayName("Loan start")]
        public DateTime LoanStart { get; set; }

        [DisplayName("Loan end")]
        public DateTime LoanEnd { get; set; }

        [DisplayName("Is oaned")]
        public bool IsLoaned { get; set; }
    }
}