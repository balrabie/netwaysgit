using System;
using System.Collections.Generic;

namespace Workshop_TecomNetways.Models
{
    public class Photo: Item
    {
        public byte[] Image { get; set; }

        public string Description { get; set; }

        public DateTime PostingDate { get; set; }

        public virtual ICollection<PhotoAlbum> PhotoAlbums { get; set; }

    }
}