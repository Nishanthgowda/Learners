using System.ComponentModel.DataAnnotations;

namespace TechTree.Models
{
    public class RegistrationModel
    {
        [Required]
        [EmailAddress]
        [Display(Name ="User Name")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Compare("Password")]                       // compares the Password property with ConfirmPassword
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        [Required]
        [StringLength(100,MinimumLength =2)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string LastName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }

        [Required]
        //[RegularExpression("^[1-9]{1}[0-9]{2}s{0, 1}[0-9]{3}$")]
        public string PostCode { get; set; }

        [Required]
        //[RegularExpression("/\b([0|+[0-9]{1,5})?([7-9][0-9]{9})\b/")]
        public string PhoneNumber { get; set; }
        public bool AcceptUserAgreement { get; set; }

        public int CategoryId { get; set; }
        public string RegistrationInValid { get; set; }
    }
}
