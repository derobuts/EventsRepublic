using IO.ClickSend.ClickSend.Api;
using IO.ClickSend.ClickSend.Model;
using IO.ClickSend.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventsRepublic.ClickSendApiall
{
    public class Clicksendapis
    {
        public void SendSms(string to,string smsmessage)
        {
            var configuration = new Configuration()
            {
               // Username = USERNAME,
               // Password = API_KEY
            };
            var smsApi = new SMSApi(configuration);

            var listOfSms = new List<SmsMessage>{ new SmsMessage(to: to,body: smsmessage,source: "sdk")
};

            var smsCollection = new SmsMessageCollection(listOfSms);
            var response = smsApi.SmsSendPost(smsCollection);
        }
    }
    
}
