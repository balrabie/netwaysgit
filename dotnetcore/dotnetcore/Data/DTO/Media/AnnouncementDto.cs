using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dotnetcore.Data
{
    public class AnnouncementDto : MediaItemDto
    {
        public string Summary { get; set; }

        public string Details { get; set; }

        public bool ShowAsPushNotification { get; set; }

        //public virtual ICollection<PeopleGroup> PeopleGroups { get; set; }
    }
}