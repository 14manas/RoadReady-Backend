namespace RoadReady.DTO
{
    public class LocationDTO
    {
        public int LocationId { get; set; }

        public string? LocationName { get; set; }
        public int? Cityid { get; set; }
        // Optionally including detailed city information
        //public CityDTO City { get; set; }
    }
}
