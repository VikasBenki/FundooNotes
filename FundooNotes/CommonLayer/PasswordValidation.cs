using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CommonLayer
{
    public class PasswordValidation
    {
        [Required(ErrorMessage = "{0} should not be empty")]
        [RegularExpression(@"(?!^[0-9]$)(?!^[a-zA-Z]$)^([a-zA-Z0-9#]{6,15})$", ErrorMessage = "Password is not valid")]
        [DataType(DataType.Password)]
        public string password { get; set; }

        [Required(ErrorMessage = "{0} should not be empty")]
        [RegularExpression(@"(?!^[0-9]$)(?!^[a-zA-Z]$)^([a-zA-Z0-9#]{6,15})$", ErrorMessage = "Password is not valid")]
        [DataType(DataType.Password)]
        //[Compare("NewPassword", ErrorMessage = "The NewPassword and ConfirmPassword do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
