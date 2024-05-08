using _4Dorms.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _4Dorms.Resources
{
    public class DormitoryDTO
    {

        [Required]
        [StringLength(150)]
        public string DormitoryName { get; set; }
        [Required]
        public string Location { get; set; }
        [Required]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        public string Amenities { get; set; }

        public DormitoryStatus Status { get; set; }

        [ForeignKey("DormitoryOwnerId")]
        public int? DormitoryOwnerId { get; set; }
    }
}
