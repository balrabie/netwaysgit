using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Workshop_TecomNetways.Models
{
    public class Criteria : Item
    {
        public virtual OnlineParticipationRequest OnlineParticipationRequest { get; set; }

        public virtual ICollection<SubCriteria> SubCriterias { get; set; }
    }
}