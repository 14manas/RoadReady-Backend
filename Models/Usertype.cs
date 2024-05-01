using System;
using System.Collections.Generic;

namespace RoadReady.Models
{
    public partial class Usertype
    {
        public Usertype()
        {
            Users = new HashSet<User>();
        }

        public int Usertypeid { get; set; }
        public string Usertypename { get; set; } = null!;

        public virtual ICollection<User> Users { get; set; }
    }
}
