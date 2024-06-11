namespace _4Dorms.Resources
{
    public class DormitorySubmitDTO
    {
        public string DormitoryName { get; set; }
        public string GenderType { get; set; }
        public string City { get; set; }
        public string Location { get; set; }
        public string NearbyUniversity { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public decimal PriceHalfYear { get; set; }
        public decimal PriceFullYear { get; set; }
        public string DormitoryDescription { get; set; }
        public RoomDTO RoomDTO { get; set; }
    }
}