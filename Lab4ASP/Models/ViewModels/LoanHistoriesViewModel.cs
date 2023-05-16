using System.ComponentModel;

namespace Lab4ASP.Models.ViewModels
{
    public class LoanHistoriesViewModel
    {
        public int LoanHistoryId { get; set; }
        public string FullName { get; set; }
        public string BookTitle { get; set; }
        public DateTime LoanStart { get; set; }
        public DateTime LoanEnd { get; set; }

        [DisplayName("Is loaned")]
        public bool Isloand { get; set; }

    }
}
