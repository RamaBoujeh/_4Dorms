using _4Dorms.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace _4Dorms.Resources
{
    public class DormitoryDTO
    {
        [Required]
        [StringLength(150)]
        public string DormitoryName { get; set; }
        public string GenderType { get; set; }
        public string City { get; set; }
        public string NearbyUniversity { get; set; }
        [Phone] 
        public string Phone { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string DormitoryDescription { get; set; }
        [Required]
        public string Location { get; set; }
        [Required]
        [DataType(DataType.Currency)]
        public decimal PriceHalfYear { get; set; }
        [Required]
        [DataType(DataType.Currency)]
        public decimal PriceFullYear { get; set; }
        public DormitoryStatus Status { get; set; }

        [ForeignKey("DormitoryOwnerId")]
        public int? DormitoryOwnerId { get; set; }
        public ICollection<string> ImageUrls { get; set; }

        // Add RoomDTO property
        public RoomDTO RoomDTO { get; set; }

        public DormitoryDTO()
        {
            ImageUrls = new List<string>();
        }
    }
}
