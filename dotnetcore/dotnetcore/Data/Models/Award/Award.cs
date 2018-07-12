﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dotnetcore.Data
{
    public class Award : Item
    {
        public byte[] Logo { get; set; }
        
        public int Number { get; set; }
    }
}