using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


//**** how to

namespace Workshop_TecomNetways.DTO
{
    public class UserTokenDto // useless Dto. Can be replaced by int UserID
    {
        public int ID { get; set; }
        //public DateTime Expiry { get; set; }
        //public bool TokenIsUsed { get; set; }
        //public string Token { get; set; }
        public int UserID { get; set; }
        //public string Email { get; set; }
    }
}