using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dotnetcore.Data
{
    public class FeedbackReceiver
    {
        public int ID { get; set; }

        public string Email { get; set; }

        public bool IsActive { get; set; }
    }
}