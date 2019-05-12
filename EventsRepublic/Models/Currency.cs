using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventsRepublic.Models
{
    public class Currency
    {
        string code;
        string name;

        public string Code { get => code; set => code = value; }
        public string Name { get => name; set => name = value; }
    }
}
