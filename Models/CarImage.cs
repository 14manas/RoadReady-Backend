using System;
using System.Collections.Generic;

namespace RoadReady.Models
{
    public partial class CarImage
    {
        public int ImageId { get; set; }
        public int? CarId { get; set; }
        public string? ImageUrl { get; set; }

        public virtual Car? Car { get; set; }
    }
}
