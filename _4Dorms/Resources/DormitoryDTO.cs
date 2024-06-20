using _4Dorms.Resources;

public class DormitoryDTO
{
    public string? DormitoryName { get; set; }
    public string? GenderType { get; set; }
    public string? City { get; set; }
    public string? Location { get; set; }
    public string? NearbyUniversity { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? DormitoryDescription { get; set; }
    public decimal? PriceHalfYear { get; set; } // Allow nullable
    public decimal? PriceFullYear { get; set; } // Allow nullable
    public List<string>? ImageUrls { get; set; }
    public RoomDTO? RoomDTO { get; set; }
}
