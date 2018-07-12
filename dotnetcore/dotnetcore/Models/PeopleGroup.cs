using System.Collections.Generic;

namespace Workshop_TecomNetways.Models
{
    public class PeopleGroup: Item
    {
        public virtual ICollection<Announcement> Announcements { get; set; }

        public virtual ICollection<Event> Events { get; set; }
    }
}