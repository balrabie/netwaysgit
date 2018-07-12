using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Workshop_TecomNetways.DTO
{
    public class VideoDto : ItemDto
    {

        public string URL { get; set; }

        public string Description { get; set; }

        public DateTime PostingDate { get; set; }

        //public virtual ICollection<VideoAlbum> VideoAlbums { get; set; }
    }
}