using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnetcore.Data
{
    public class VideoAlbumVideo
    {
        public int VideoID { get; set; }

        public int VideoAlbumID { get; set; }

        public virtual Video Video { get; set; }

        public virtual VideoAlbum VideoAlbum { get; set; }
    }
}
