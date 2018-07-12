using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Workshop_TecomNetways.Models
{
    public class FeedbackReceiver
    {
        public int ID { get; set; }

        public string Email { get; set; }

        public bool IsActive { get; set; }
    }
}