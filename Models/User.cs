using System;
using System.Collections.Generic;

namespace RoadReady.Models
{
    public partial class User
    {
        public User()
        {
            CarReviews = new HashSet<CarReview>();
            Cars = new HashSet<Car>();
            Reservations = new HashSet<Reservation>();
        }

        public int UserId { get; set; }
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Firstname { get; set; } = null!;
        public string Lastname { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public DateTime Dob { get; set; }
        public int? Usertypeid { get; set; }

        public virtual Usertype? Usertype { get; set; }
        public virtual ICollection<CarReview> CarReviews { get; set; }
        public virtual ICollection<Car> Cars { get; set; }
        public virtual ICollection<Reservation> Reservations { get; set; }
    }
}
