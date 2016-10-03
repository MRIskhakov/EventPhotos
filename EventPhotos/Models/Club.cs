using System;
using System.Collections.Generic;

namespace EventPhotos.Models
{
    public class Club
    {
        public Guid Id { get; set; }
        public String Name { get; set; }

        public virtual ICollection<Event> Events { get; set; }
    }
}