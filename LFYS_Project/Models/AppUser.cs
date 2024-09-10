using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace LFYS_Project.Models
{
    public class AppUser  : IdentityUser
    {
        [StringLength(100)]
        [MaxLength(100)]
        [Required]
        public string? Name { get; set; }
        public string? Address { get; set; }
        [StringLength(200)]
        [MaxLength(200)]


        public string? Facebook { get; set; }
        [StringLength(200)]
        [MaxLength(200)]

        public string? Youtube { get; set; }
        [StringLength(200)]
        [MaxLength(200)]


        public string? Instagram { get; set; }
        [StringLength(200)]
        [MaxLength(200)]


        public string? Avatar { get; set; }
    }
}
