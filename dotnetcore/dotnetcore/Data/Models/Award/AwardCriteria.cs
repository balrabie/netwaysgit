using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dotnetcore.Data
{
    public class AwardCriteria : Item
    {
        public byte[] Logo { get; set; }

        public double Weight { get; set; } // do i put constraint on min max?
    }
}