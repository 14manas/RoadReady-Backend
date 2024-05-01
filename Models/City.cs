using System;
using System.Collections.Generic;

namespace RoadReady.Models
{
    public partial class City
    {
        public City()
        {
            Locations = new HashSet<Location>();
        }

        public int Cityid { get; set; }
        public string Cityname { get; set; } = null!;
        public int? StateId { get; set; }

        public virtual State? State { get; set; }
        public virtual ICollection<Location> Locations { get; set; }
    }
}
