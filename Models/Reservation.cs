using System;
using System.Collections.Generic;

namespace RoadReady.Models
{
    public partial class Reservation
    {
        public Reservation()
        {
            PaymentDetails = new HashSet<PaymentDetail>();
        }

        public int ReservationId { get; set; }
        public int? UserId { get; set; }
        public int? CarId { get; set; }
        public DateTime ReservationDateTime { get; set; }
        public DateTime ReturnDateTime { get; set; }
        public decimal TotalCost { get; set; }
        public string? StatusName { get; set; }

        public virtual Car? Car { get; set; }
        public virtual User? User { get; set; }
        public virtual ICollection<PaymentDetail> PaymentDetails { get; set; }
    }
}
