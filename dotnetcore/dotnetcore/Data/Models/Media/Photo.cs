using System;
using System.Collections.Generic;

namespace dotnetcore.Data
{
    public class Photo: Item
    {
        public byte[] Image { get; set; }

        public string Description { get; set; }

        public DateTime PostingDate { get; set; }

        public virtual ICollection<PhotoAlbumPhoto> PhotoAlbums { get; set; }

    }
}