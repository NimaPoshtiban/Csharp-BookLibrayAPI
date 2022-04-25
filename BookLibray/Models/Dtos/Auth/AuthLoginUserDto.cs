using System.ComponentModel.DataAnnotations;

namespace BookLibrary.Models.Dtos.Auth
{
    public class AuthLoginUserDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
