using System.ComponentModel.DataAnnotations;

namespace Plashoe.DTOs
{
    public class RegisterDTO
    {
        [Required]
        public string? Name { get; set; }
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        public string? Password { get; set; }
        [Required]
        [Compare("Password", ErrorMessage = "password and confirmation password do not match.")]
        public string? CPassword { get; set; }
    }
}
