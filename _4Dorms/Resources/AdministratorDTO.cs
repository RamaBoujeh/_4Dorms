using System.ComponentModel.DataAnnotations;

namespace _4Dorms.Resources
{
    public class AdministratorDTO
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string PhoneNumber { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
