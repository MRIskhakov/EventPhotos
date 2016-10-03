using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EventPhotos.Models
{
    public class Photo
    {
        public Photo()
        {
            PhotoID = Guid.NewGuid();
        }

        public Guid PhotoID { get; set; }

        public string Name { get; set; }

        public DateTime CreatedAt { get; set; }

        public virtual Event Event { get; set; }

        public String photoPath()
        {
            return String.Format("/{0}/{1}.jpg", Constants.UPLOADS_FOLDER, this.PhotoID.ToString());
        }
    }
}