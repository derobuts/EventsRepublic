using Nexmo.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventsRepublic.NexmoSms
{
    public class Nexmosms
    {
        public void SendSms()
        {
            var client = new Client(creds: new Nexmo.Api.Request.Credentials
            {
                
            });
            var results = client.SMS.Send(request: new SMS.SMSRequest
            {

                from ="dbutoyez",
                to = "254703567954",
                text = "Hello"
            });
            var h = 1;
        }
    }
}
