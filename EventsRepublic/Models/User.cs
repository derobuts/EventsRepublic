using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventsRepublic.Models
{
    public class UserInfo
    {
        private string username;
        private int userid;
        private string email;
        private string phoneno;
        private int role;
        private string pwd;
        private int orderid;
        private string passwdhash;
        private string salt;
        private string creationdate;
        private string loginprovider;
        private string timezone;
     
        public string UserName { get => username; set => username = value; }
        public int Role { get => role; set => role = value; }
        public int UserId { get => userid; set => userid = value; }
        public string Pwd { get => pwd; set => pwd = value; }
        public string Email { get => email; set => email = value; }
        public string Phoneno { get => phoneno; set => phoneno = value; }
        public string Passwdhash { get => passwdhash; set => passwdhash = value; }
        public string Salt { get => salt; set => salt = value; }
        public string Creationdate { get => creationdate; set => creationdate = value; }
        public string Loginprovider { get => loginprovider; set => loginprovider = value; }
        public string Timezone { get => timezone; set => timezone = value; }
      //  public int Orderid { get => orderid; set => orderid = value; }
    }
}
