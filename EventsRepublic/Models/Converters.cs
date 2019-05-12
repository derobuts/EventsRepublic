using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventsRepublic.Models
{
    public class Converters
    {
        public object MpesaValUrl(object value)
        {
            var k = value;
            MpesaClassValidation valclass = new MpesaClassValidation();

            var data = (JObject)JsonConvert.DeserializeObject(value.ToString());
            valclass.MSISDN = data["MSISDN"].Value<string>();
            valclass.BillRef = data["BillRefNumber"].Value<int>();
            valclass.TransAmount = data["TransAmount"].Value<decimal>();
            return valclass;
        }
        //

    }
}
