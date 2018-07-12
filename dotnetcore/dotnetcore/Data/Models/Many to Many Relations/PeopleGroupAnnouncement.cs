using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnetcore.Data
{
    public class PeopleGroupAnnouncement
    {
        public int PeopleGroupID { get; set; }

        public int AnnouncementID { get; set; }

        public virtual PeopleGroup PeopleGroup { get; set; }

        public virtual Announcement Announcement { get; set; }
    }


}
