using System.ComponentModel.DataAnnotations;

namespace _4Dorms.Models
{
    public class DormitoryOwner
    {
        [Key]
        public int DormitoryOwnerId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string Gender { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public virtual ICollection<FavoriteList> Favorites { get; set; }

        public virtual ICollection<Dormitory> Dormitories { get; set; }

        public virtual ICollection<Booking> Bookings { get; }

        public DormitoryOwner()
        {
            Favorites = new HashSet<FavoriteList>();
            Dormitories = new HashSet<Dormitory>();
            Bookings = new HashSet<Booking>();
        }
    }
}
