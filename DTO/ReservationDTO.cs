using System.Text.Json.Serialization;

namespace RoadReady.DTO
{
    public class ReservationDTO
    {
        public int ReservationId { get; set; }
        public DateTime ReservationDateTime { get; set; }
        public DateTime ReturnDateTime { get; set; }
        public decimal TotalCost { get; set; }
        public string StatusName { get; set; }
        public int UserId { get; set; } 
        public int CarId { get; set; }  
        
        public UserDTO User { get; set; }
      
        public CarDTO Car { get; set; }
    }
}
