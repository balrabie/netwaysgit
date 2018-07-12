using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Workshop_TecomNetways.DTO
{
    public class AwardQuoteDto : ItemDto
    {
        public byte[] Image { get; set; }

        public string Description { get; set; }

        public string ContactName { get; set; }

        public string JobTitle { get; set; }
    }
}