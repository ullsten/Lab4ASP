﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNet.Identity;

namespace Lab4ASP.Models.ViewModels
{
    public class UserBookViewModel
    {
        public int LoanHistoryId { get; set; }

        [DisplayName("User")]
        public string? UserName { get; set; }

        [DisplayName("Book Title")]
        public string? BookTitle { get; set; }

        [DisplayName("Book Title: ")]
        public string? RndTitle { get; set; }

        [DisplayName("Description")]
        public string? BookDescription { get; set; }

        [DisplayName("Description: ")]
        public string? RndDescription { get; set; }

        public string Author { get; set; }
        public byte[] BookPicture { get; set; }

        [DisplayName("Loaned")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? LoanStart { get; set; }

        [DisplayName("To be returned")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? LoanEnd { get; set; }

        [DisplayName("Loaned")]
        public bool IsLoaned { get; set; }
        [DisplayName("Returned")]
        public bool IsReturned { get; set; }

        [DisplayName("Returned Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? ReturnedDate { get; set; }

        public string Borrower { get; set; }

        [DisplayName("Days left")]
        [NotMapped]
        public int DaysLeft
        {
            get
            {
                if (!IsReturned)
                {
                    TimeSpan remainingTime = (TimeSpan)(LoanEnd - DateTime.Now);
                    return remainingTime.Days;
                }
                else
                {
                    return 0;
                }
            }
        }

        //Relation for borrow book
        [DisplayName("User")]
        public string FK_UserId { get; set; }
        public ApplicationUser? Users { get; set; }

        [DisplayName("Book")]
        public int FK_BookId { get; set; }
        public Book? Books { get; set; }

        public int FK_AuthorId { get; set; }
        public Author? Authors { get; set; }
      
    }
}
