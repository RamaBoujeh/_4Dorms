using System.ComponentModel.DataAnnotations;

namespace _4Dorms.Models
{
    public class DormitoryImage
    {
        [Key]
        public int ImageId { get; set; }
        public string Url { get; set; }
        public int DormitoryId { get; set; }
        public Dormitory Dormitory { get; set; }
    }
}
