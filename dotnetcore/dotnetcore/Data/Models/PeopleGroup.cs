using System.Collections.Generic;

namespace dotnetcore.Data
{
    public class PeopleGroup: Item
    {
        public virtual ICollection<PeopleGroupAnnouncement> Announcements { get; set; }

        public virtual ICollection<PeopleGroupEvent> Events { get; set; }
    }
}