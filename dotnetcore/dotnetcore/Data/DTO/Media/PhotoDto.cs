﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dotnetcore.Data
{
    public class PhotoDto : ItemDto
    {
        public byte[] Image { get; set; }

        public string Description { get; set; }

        public DateTime PostingDate { get; set; }

        //public virtual ICollection<PhotoAlbum> PhotoAlbums { get; set; }

    }
}