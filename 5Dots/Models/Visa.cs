using System;
using System.ComponentModel.DataAnnotations;

namespace _5Dots.Models
{
    public class Visa
    {
        public int VisaId { get; set; }

        [Required(ErrorMessage = "CVC is required")]
        [Range(100, 999, ErrorMessage = "CVC must be a 3-digit number")]
        public short CVC { get; set; }

        [Required(ErrorMessage = "Visa number is required")]
        [CreditCard(ErrorMessage = "Invalid credit card number")]
        public string VisaNumber { get; set; }

        [Required(ErrorMessage = "Expiration date is required")]
        [DataType(DataType.Date)]
        [StringLength(16, MinimumLength = 16, ErrorMessage = "Visa number must be exactly 16 characters")]
        [DisplayFormat(DataFormatString = "{0:MM/yyyy}", ApplyFormatInEditMode = true)]
       
        public DateTime ExpDate { get; set; }

        [Required(ErrorMessage = "User ID is required")]
        public string UserId { get; set; }

        public User User { get; set; }
    }
}
