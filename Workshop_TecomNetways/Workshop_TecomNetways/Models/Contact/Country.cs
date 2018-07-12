using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Workshop_TecomNetways.Models
{
    public class Country
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public string CountryCode { get; set; }

        public virtual ICollection<FeedbackRequest> FeedbackRequests { get; set; }

        public virtual ICollection<OnlineParticipationRequest> OnlineParticipationRequests { get; set; }
    }
}