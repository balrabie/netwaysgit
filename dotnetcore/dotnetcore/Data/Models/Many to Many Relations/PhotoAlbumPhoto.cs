using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnetcore.Data
{
    public class PhotoAlbumPhoto
    {
        public int PhotoID { get; set; }

        public int PhotoAlbumID { get; set; }

        public virtual Photo Photo { get; set; }

        public virtual PhotoAlbum PhotoAlbum { get; set; }
    }
}
