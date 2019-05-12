using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsRepublic.Models.Mpesa
{
    public static class MPESA
    {  
        public class consumer2business
        {
            public string BusinessShortCode;
            public string Password;
            public string Timestamp;
            public string TransactionType;
            public string Amount;
            public string PartyA;
            public string PartyB;
            public string PhoneNumber;
            public string CallBackURL;
            public string AccountReference;
            public string TransactionDesc;
        }
    }
}
