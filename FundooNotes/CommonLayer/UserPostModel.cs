using System;
using System.ComponentModel.DataAnnotations;

namespace CommonLayer
{
    public class UserPostModel
    {
        [Required]
        [RegularExpression(@"^[A-Z]{1}[a-z]{4,}$", ErrorMessage = "name starts with Cap and has minimum 8 characters")]
        public string firstName { get; set; }

        [Required]
        [RegularExpression(@"^[A-Z]{1}[a-z]{4,}$", ErrorMessage = "name starts with Cap and has minimum 8 characters")]
        public string lastName { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z0-9]{3,}([._+-][a-zA-Z0-9]{1,})?@[a-zA-Z0-9]{1,10}[.][a-zA-Z]{2,3}([.][a-zA-Z]{2,3})?$", ErrorMessage = "Email Id is not valid")]
        public string email { get; set; }
        public DateTime registerdDate { get; set; }

        [Required]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[&%$#@?^*!~]).{8,}$", ErrorMessage = "Passsword is not valid")]
        public string password { get; set; }
        public string address { get; set; }
    }
}
