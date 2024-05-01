namespace RoadReady.DTO
{
    public class CarDTO
    {
        public int CarId { get; set; }
        public int? AgentId { get; set; }
        public decimal RatePerHour { get; set; }
        public int Available { get; set; }
        public int LocationId { get; set; }
        public List<CarDetailDTO> CarDetails { get; set; }
        public List<CarImageDTO> CarImages { get; set; }

        
    }
}
