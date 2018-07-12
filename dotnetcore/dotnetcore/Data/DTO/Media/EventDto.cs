using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dotnetcore.Data
{
    public class EventDto : MediaItemDto
    {
        public bool AllDayEvent { get; set; }

        public string OrganisedBy { get; set; }

        public int ContactNumber { get; set; }

        public string Email { get; set; }

        public string Details { get; set; }

        public string Category { get; set; }

        public string URL { get; set; }
        
        public int LocationID { get; set; }

        //public virtual Location Location { get; set; }

        //public virtual ICollection<PeopleGroup> PeopleGroups { get; set; }
    }
}
