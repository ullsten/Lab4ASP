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
        public string FK_UserId { get; set; }
        public ApplicationUser? Users { get; set; }

        [ForeignKey("Books")]
        [DisplayName("Book Id")]
        public int FK_BookId { get; set; }
        public Book? Books { get; set; }

        [Required]
        [DisplayName("Loan start")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime LoanStart { get; set; } = DateTime.Now;

        [Required]
        [DisplayName("Loan end")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime LoanEnd => LoanStart.AddDays(9);


        [Required]
        [DisplayName("Loan again")]
        public bool IsLoaned { get; set; }

        [DisplayName("Returned")]
        public bool IsReturned { get; set; } = false; //false as default
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]

        [DisplayName("Returned Date")]
        public DateTime? ReturnedDate { get; set; }
        [NotMapped]
        public string ReturnedDateMessage
        {
            get
            {
                if (IsReturned)
                {
                    return ReturnedDate?.ToString("yyyy-MM-dd") ?? "Not available";
                }
                else
                {
                    return "Not returned";
                }
            }
        }

        [NotMapped]
        public int DaysLeft
        {
            get
            {
                if (!IsReturned)
                {
                    TimeSpan remainingTime = LoanEnd - DateTime.Now;
                    return remainingTime.Days;
                }
                else
                {
                    return 0;
                }
            }
        }
    }
}