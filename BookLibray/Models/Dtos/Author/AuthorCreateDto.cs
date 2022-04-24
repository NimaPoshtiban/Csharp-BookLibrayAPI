using System.ComponentModel.DataAnnotations;

namespace BookLibray.Models.Dtos.Author
{
    public class AuthorCreateDto
    {
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }
     
        [MaxLength(250)]
        public string Bio { get; set; }
    }
}
