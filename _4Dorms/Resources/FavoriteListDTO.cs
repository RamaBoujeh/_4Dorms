using System.ComponentModel.DataAnnotations;

namespace _4Dorms.Resources
{
    public class FavoriteListDTO
    {
        public int FavoriteListId { get; set; }
        [Required]
        public int DormitoryId { get; set; }
    }
}
