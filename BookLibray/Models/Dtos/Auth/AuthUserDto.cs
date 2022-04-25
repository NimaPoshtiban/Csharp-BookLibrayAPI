using System.ComponentModel.DataAnnotations;

namespace BookLibrary.Models.Dtos.Auth
{
    public class AuthUserDto : AuthLoginUserDto
    {
        [Required]
        public string Role { get; set; }
    }
}
