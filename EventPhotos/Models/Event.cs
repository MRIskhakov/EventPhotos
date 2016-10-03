using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EventPhotos.Models
{
    public enum Type
    {
        Party, Birthday, Concert
    }

    public class Event
    {
        public Event()
        {
            Photos = new List<Photo>();
        }

        public Guid EventID { get; set; }

        public string Name { get; set; }

        public DateTime Date { get; set; }

        public string Place { get; set; }

        public Club Club { get; set; }

        public Type Type { get; set; }

        public virtual ICollection<Photo> Photos { get; set; }

        public Photo getRandomPhoto()
        {
            if (Photos.Count == 0) { return null; }
            Random random = new Random();
            var index = random.Next(0, this.Photos.Count) % this.Photos.Count;
            return this.Photos.ElementAt(index);
        }
    }
}