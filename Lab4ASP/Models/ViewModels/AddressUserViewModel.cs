using System.ComponentModel;

namespace Lab4ASP.Models.ViewModels
{
    public class AddressUserViewModel
    {
        [DisplayName("Address Id")]
        public int AddressId { get; set; }
        public string Street { get; set; }
        [DisplayName("Postal Code")]
        public string PostalCode { get; set; }
        public string City { get; set; }
        [DisplayName("User Id")]
        public string UserId { get; set; }
        [DisplayName("User")]
        public string UserName { get; set; }
    }
}
