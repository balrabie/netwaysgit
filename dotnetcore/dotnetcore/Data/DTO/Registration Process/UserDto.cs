using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dotnetcore.Data
{
    public class UserDto
    { 
        public int ID { get; set; }

        public string Email { get; set; }

        public string Password { get; set; } // hashed

        //public byte[] Salt { get; set; }

        //public bool PasswordsMatch { get; set; }

        public string PassportNumber { get; set; }

        public string Gender { get; set; } 

        public string SchoolName { get; set; }

        public string TeachingArea { get; set; }

        public int NationalityID { get; set; }

        //public virtual Nationality Nationality { get; set; }

        public int PeopleGroupID { get; set; }

        //public virtual PeopleGroup PeopleGroup { get; set; }

    }
}