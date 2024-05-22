using System.ComponentModel.DataAnnotations;

namespace _4Dorms.Models
{
    public class Administrator
    {
        [Key]
        public int AdministratorId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]

        public string PhoneNumber { get; set; }
        public string Password { get; set; }

        public string? ProfilePictureUrl { get; set; }
    }
}
