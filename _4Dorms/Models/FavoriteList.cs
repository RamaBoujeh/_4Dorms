using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace _4Dorms.Models
{
    public class FavoriteList
    {
        [Key]
        public int FavoriteId { get; set; }
        public Student Student { get; set; }
        [ForeignKey("StudentId")]
        public int? StudentId { get; set; }
        public DormitoryOwner DormitoryOwner { get; set; }
        [ForeignKey("DormitoryOwnerId")]
        public int? DormitoryOwnerId { get; set; }
        public virtual ICollection<Dormitory> Dormitories { get; set; }

        public FavoriteList()
        {
            Dormitories = new HashSet<Dormitory>();
        }
    }
}
