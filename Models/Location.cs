using System;
using System.Collections.Generic;

namespace RoadReady.Models
{
    public partial class Location
    {
        public Location()
        {
            Cars = new HashSet<Car>();
        }

        public int LocationId { get; set; }
        public int? Cityid { get; set; }
        public string? LocationName { get; set; }

        public virtual City? City { get; set; }
        public virtual ICollection<Car> Cars { get; set; }
    }
}
