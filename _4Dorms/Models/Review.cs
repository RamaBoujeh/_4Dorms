using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace _4Dorms.Models
{
    public class Review
    {
        [Key]
        public int ReviewId { get; set; }
        [StringLength(200)]
        public string Comment { get; set; }
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        public int Rating { get; set; }
        [ForeignKey("DormitoryId")]
        public int? DormitoryId { get; set; }
        public Dormitory Dormitory { get; set; }
        [ForeignKey("StudentId")]
        public int? StudentId { get; set; }
        public Student Student { get; set; }
    }
}
