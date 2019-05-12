using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventsRepublic.Models
{
    public class JwtToken
    {
        public class Header
        {
            public string typ;
            public string alg;
        }
        public class Body
        {
            public string Iss;
            public string exp;
            public Guid App_ID;
            public string Role;
            public int User_Id;
            public string idp;
        }
    }
}
