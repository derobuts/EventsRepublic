using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventsRepublic.Database
{
    public class DBCOntext
    {
        public string GetdbConnectionString()
        {
            string dbcon = "Data Source=DESKTOP-AGSVP0O\\SQLEVENTV3;Initial Catalog=Eventdb;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            return dbcon;
        }
        public string GetUserdbConnectionString()
        {
            string dbcon = "Data Source=(localdb)\\TicketT;Initial Catalog=Tickot;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            return dbcon;
        }
    }
}
