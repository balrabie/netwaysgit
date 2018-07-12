using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dotnetcore.Data
{
    public class UserToken
    {
        public int ID { get; set; }

        public DateTime Expiry { get; set; }

        public bool TokenIsUsed { get; set; }

        public string Token { get; set; } // those should not be set by user

        public int UserID { get; set; }

        public string Email { get; set; } // cant be modified after set

        public virtual User User { get; set; }   
    }
}