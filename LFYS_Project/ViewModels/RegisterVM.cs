using Newtonsoft.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace LFYS_Project.ViewModels
{
    public class RegisterVM
    {
        [Required]
        public string? Name { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }
        [DataType(DataType.Password)]
        public string? Password { get; set; }
        [Compare("Password", ErrorMessage = "Password do not match")]
        public string? ConfirmPassword { get; set; }
        public string? Address { get; set; }

    }
}
