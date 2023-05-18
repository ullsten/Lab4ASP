using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Lab4ASP.Models.ViewModels
{
    public class UserBookViewModel
    {
        [DisplayName("Borrower")]
        public string UserName { get; set; }

        [DisplayName("Book Title")]
        public string BookTitle { get; set; }
        
        [DisplayName("Book Title: ")]
        public string RndTitle { get; set; }

        [DisplayName("Description")]
        public string BookDescription { get; set; }
        
        [DisplayName("Description: ")]
        public string RndDescription { get; set; }

        [DisplayName("Loaned")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime LoanStart { get; set; }

        [DisplayName("Last loan day")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime LoanEnd { get; set; } = DateTime.Now;
    }
}
