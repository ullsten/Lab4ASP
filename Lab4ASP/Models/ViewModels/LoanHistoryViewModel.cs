using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Lab4ASP.Models.ViewModels
{
    public class LoanHistoryViewModel
    {
        public int LoanHistoryId { get; set; }

        [DisplayName("Borrower")]
        public string FullName { get; set; }

        [DisplayName("Title")]
        public string BookTitle { get; set; }


        [DisplayName("Loan start")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime LoanStart { get; set; }

        [DisplayName("Last loan day")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime LoanEnd { get; set; }

        [DisplayName("Is loaned")]
        public bool Isloand { get; set; }

    }
}
