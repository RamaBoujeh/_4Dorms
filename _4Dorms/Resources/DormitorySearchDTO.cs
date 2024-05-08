namespace _4Dorms.Resources
{
    public class DormitorySearchDTO
    {
        public string? DormitoryName { get; set; }
        public string? Location { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public bool SortByPriceAscending { get; set; }
        public bool SortByPriceDescending { get; set; }
    }
}
