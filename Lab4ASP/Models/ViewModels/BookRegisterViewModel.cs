using System.ComponentModel;

namespace Lab4ASP.Models.ViewModels
{
    public class BookRegisterViewModel
    {
        [DisplayName("ID")]
        public int BookId { get; set; }

        [DisplayName("Title")]
        public string BookTitle { get; set; }

        [DisplayName("Description")]
        public string BookDescription { get; set; }
        public int Published { get; set; }
        public string Genre { get; set; }
        public string Author { get; set; }
        public int Quantity { get; set; }
    }
}
