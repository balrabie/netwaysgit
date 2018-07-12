﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dotnetcore.Data
{
  
    public class MediaItem: Item
    {
        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public byte[] Image { get; set; }
    }
}