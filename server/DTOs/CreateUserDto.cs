using System.ComponentModel.DataAnnotations;

namespace server.DTOs
{
    public class CreateUserDto
    {
        [Required]
        [MinLength(3)]
        [MaxLength(100)]
        public string Login { get; set; } = string.Empty;

        [Required]
        [MinLength(6)]
        public string Password { get; set; } = string.Empty;

        [Required]
        [MinLength(2)]
        [MaxLength(200)]
        public string FullName { get; set; } = string.Empty;

        [EmailAddress]
        [MaxLength(200)]
        public string? Email { get; set; }

        [MaxLength(150)]
        public string? Position { get; set; }

        [Required]
        public string Role { get; set; } = string.Empty;
    }
}
