namespace RoadReady.DTO
{
    public class CarReviewDTO
    {
        public int ReviewId { get; set; }
        public int Rating { get; set; }
        public string ReviewText { get; set; }
        public DateTime Timestamp { get; set; }
        // For context,we can  include UserId and CarId
        public int? UserId { get; set; }
        public int? CarId { get; set; }
    }
}
