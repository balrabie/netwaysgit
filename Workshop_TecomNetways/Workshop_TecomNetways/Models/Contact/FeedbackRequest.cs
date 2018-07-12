using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Workshop_TecomNetways.Models
{
    public enum Status
    {
        Unknown,
        Open,
        InProgress,
        Closed
    }

    public class FeedbackRequest
    {
        public int ID { get; set; }

        public string Submitter { get; set; }

        public string MobileNumber { get; set; }

        public string Email { get; set; }

        public string Subject { get; set; }

        public string Details { get; set; }
        
        public int CountryID { get; set; }

        public virtual Country Country { get; set; }

        public Status Status { get; set; } = Status.Unknown; //will be set a different API***

        
    }
}