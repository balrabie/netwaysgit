using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Workshop_TecomNetways.DTO
{
    public class AlbumDto<T> : ItemDto where T : class
    {
        public byte[] CoverImage { get; set; }

        public string Details { get; set; }

        //public virtual ICollection<T> AlbumItems { get; set; }
    }
}