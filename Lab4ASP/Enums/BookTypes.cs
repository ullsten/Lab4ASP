using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Lab4ASP.Enums
{
    public enum BookTypes
    {
        Fiction,
        Biography,
        Mystery,
        Romance,
        [Display(Name = "Science Fiction")]
        ScienceFiction,
        History,
        [Display(Name = "Cook Book")]
        CookBook,
        Philosophy,
    }
}
