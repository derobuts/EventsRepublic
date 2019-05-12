using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventsRepublic.Models
{
    public class Tokens
    {
        public Guid RefreshToken { get; set; }
        public string jwtToken { get; set; }
    }
}
