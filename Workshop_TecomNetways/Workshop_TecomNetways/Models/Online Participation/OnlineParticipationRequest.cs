using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Workshop_TecomNetways.Models
{
    public class OnlineParticipationRequest: Item
    {
        public string TrackingCode { get; set; }

        public int UserID { get; set; }

        public int CountryID { get; set; }

        public virtual User User { get; set; }

        public virtual Country Country { get; set; }

        public virtual ICollection<Criteria> Criterias { get; set; }
    }
}