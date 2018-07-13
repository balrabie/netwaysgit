using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnetcore.Controllers
{
    public class ChangePasswordInfo
    {
        public string Token { get; set; }

        public string NewPassword { get; set; }
    }
}
