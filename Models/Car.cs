using System;
using System.Collections.Generic;

namespace RoadReady.Models
{
    public partial class Car
    {
        public Car()
        {
            CarDetails = new HashSet<CarDetail>();
            CarImages = new HashSet<CarImage>();
            CarReviews = new HashSet<CarReview>();
            Reservations = new HashSet<Reservation>();
        }

        public int CarId { get; set; }
        public int? AgentId { get; set; }
        public int? LocationId { get; set; }
        public decimal RatePerHour { get; set; }
        public int Available { get; set; }
        public int? CityId { get; set; }

        public virtual User? Agent { get; set; }
        public virtual Location? Location { get; set; }
        public virtual ICollection<CarDetail> CarDetails { get; set; }
        public virtual ICollection<CarImage> CarImages { get; set; }
        public virtual ICollection<CarReview> CarReviews { get; set; }
        public virtual ICollection<Reservation> Reservations { get; set; }
    }
}
