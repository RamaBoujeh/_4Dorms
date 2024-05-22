using _4Dorms.Models;
using System.ComponentModel.DataAnnotations;

namespace _4Dorms.Resources
{
    public enum UserType
    {
        Student,
        DormitoryOwner,
        Administrator
    }
    public class SignUpDTO
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Passwords do not match")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [Phone(ErrorMessage = "Invalid phone number")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Gender is required")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Date of birth is required")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "User type is required")]
        public UserType UserType { get; set; }

        public string? Disabilities { get; set; }
        public string? ProfilePictureUrl { get; set; }

        // public bool CreateFavoriteList { get; set; }

    }
}
