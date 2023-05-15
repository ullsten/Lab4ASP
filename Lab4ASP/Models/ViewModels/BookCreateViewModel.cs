using Lab4ASP.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Lab4ASP.Models.ViewModels
{
    public class BookCreateViewModel
    {
        public int BookId { get; set; }
        public string? BookTitle { get; set; }
        public string? BookDescription { get; set; }
        public string? PublishedYear { get; set; }
        //public IEnumerable<string>? BookTypes { get; set; } //list of book types
        public List<SelectListItem> BookTypes { get; set; }
    }
}
