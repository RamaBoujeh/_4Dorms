using _4Dorms.Models;
using _4Dorms.Resources;
using System.ComponentModel.DataAnnotations;

public class BookingDTO
{
    [Required]
    public int RoomId { get; set; }

    [Required]
    public int DormitoryId { get; set; }

    [Required]
    public int StudentId { get; set; }

    [Required]
    public Duration Duration { get; set; }

    [Required]
    public RoomType RoomType { get; set; }

}