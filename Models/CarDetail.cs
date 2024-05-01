using System;
using System.Collections.Generic;

namespace RoadReady.Models
{
    public partial class CarDetail
    {
        public int CarDetailId { get; set; }
        public int? CarId { get; set; }
        public string Make { get; set; } = null!;
        public string Model { get; set; } = null!;
        public int Year { get; set; }
        public string Color { get; set; } = null!;
        public string Description { get; set; } = null!;

        public virtual Car? Car { get; set; }
    }
}
