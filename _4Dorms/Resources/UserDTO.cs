namespace _4Dorms.Resources
{
    public class UserDTO
    {
        public int UserId { get; set; }  // User ID of the profile being updated
        public UserType UserType { get; set; }  // Type of user (e.g., Student, DormitoryOwner, etc.)

        // Common profile update properties
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string? Disabilities { get; set; }
        public string? ProfilePictureUrl { get; set; }

    }
}
