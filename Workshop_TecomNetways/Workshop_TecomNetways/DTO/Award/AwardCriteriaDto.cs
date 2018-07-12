using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Workshop_TecomNetways.DTO
{
    public class AwardCriteriaDto : ItemDto
    {
        public byte[] Logo { get; set; }

        public double Weight { get; set; } // do i put constraint on min max?
    }
}