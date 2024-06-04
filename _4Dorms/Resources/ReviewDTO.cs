using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _4Dorms.Resources
{
    public class ReviewDTO
    {
        [ForeignKey("DormitoryId")]
        [Required(ErrorMessage = "DormitoryId is required")]
        public int DormitoryId { get; set; }

        [Required(ErrorMessage = "StudentId is required")]
        [ForeignKey("StudentId")]
        public int StudentId { get; set; }

        [Required(ErrorMessage = "Rating is required")]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5")]
        public int Rating { get; set; }

        public string Comment { get; set; }

        public DateTime Date { get; set; }

        public string StudentName { get; set; } // Assuming Review has a navigation property to Student
    }
}
