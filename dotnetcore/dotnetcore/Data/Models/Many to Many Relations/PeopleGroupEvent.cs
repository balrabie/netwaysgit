using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnetcore.Data
{
    public class PeopleGroupEvent
    {
        public int PeopleGroupID { get; set; }

        public int EventID { get; set; }

        public virtual PeopleGroup PeopleGroup { get; set; }

        public virtual Event Event { get; set; }
    }
}
