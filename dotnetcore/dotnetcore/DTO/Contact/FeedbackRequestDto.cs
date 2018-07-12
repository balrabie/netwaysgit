using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Workshop_TecomNetways.DTO
{
    public class FeedbackRequestDto
    {
        public int ID { get; set; }

        public string Submitter { get; set; }

        public string MobileNumber { get; set; }

        public string Email { get; set; }

        public string Subject { get; set; }

        public string Details { get; set; }
        
        public int CountryID { get; set; }

        //public virtual Country Country { get; set; }

    }
}