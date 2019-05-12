using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventsRepublic.Models.Mpesa
{
    public static class JSONClass
    {
        public static string ObjectToJson<T>(T t)
        {
            return JsonConvert.SerializeObject(t);
        }
    }
}
