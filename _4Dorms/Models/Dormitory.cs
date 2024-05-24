using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace _4Dorms.Models
{
    public enum DormitoryStatus
    {
        Pending,
        Approved,
        Rejected
    }

    public class Dormitory
    {
        [Key]
        public int DormitoryId { get; set; }
        [Required]
        [StringLength(150)]
        public string DormitoryName { get; set; }
        public string GenderType { get; set; }
        public string City { get; set; }
        public string NearbyUniversity { get; set; }
        [Phone]
        public string phone {  get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string DormitoryDescription { get; set;}
        [Required]
        public string Location { get; set; }
        [Required]
        [DataType(DataType.Currency)]
        public decimal PriceHalfYear { get; set; }

        public decimal PriceFullYear { get; set; }

        [ForeignKey("DormitoryOwnerId")]
        public int? DormitoryOwnerId { get; set; }
        public DormitoryOwner DormitoryOwner { get; set; }

        [ForeignKey("AdministratorId")]
        public int? AdministratorId { get; set; }

        public Administrator Administrator { get; set; }

        public DormitoryStatus Status { get; set; }

        public virtual ICollection<Room> Rooms { get; set; }

        public virtual ICollection<Review> Reviews { get; set; }

        public virtual ICollection<Booking> Bookings { get; set; }

        public virtual ICollection<FavoriteList> Favorites { get; set; }
        public ICollection<DormitoryImage> ImageUrls { get; set; }

        public Dormitory()
        {
            Rooms = new HashSet<Room>();
            Reviews = new HashSet<Review>();
            Bookings = new HashSet<Booking>();
            Favorites = new HashSet<FavoriteList>();
            ImageUrls = new HashSet<DormitoryImage>();
        }
    }
}

