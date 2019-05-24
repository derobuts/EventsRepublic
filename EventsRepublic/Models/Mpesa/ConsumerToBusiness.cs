using EventsRepublic.InterFace;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventsRepublic.Models.Mpesa
{
    /**
    * consumer to business api
    * validation url doesn't seem to be called
   **/
    public class ConsumerToBusiness
    {
        public string TransactionType { get; set; }
        public string TransID { get; set; }
        public string TransTime { get; set; }
        public string TransAmount { get; set; }
        public string BusinessShortCode { get; set; }
        public string BillRefNumber { get; set; }
        public string InvoiceNumber { get; set; }
        public string OrgAccountBalance { get; set; }
        public string ThirdPartyTransID { get; set; }
        public string MSISDN { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }

        public async static void Registerc2burl()
        {
            //create an anonymous object
            var registerurl = new
            {
                ShortCode = "603083",
                ResponseType = "Cancelled",
                ConfirmationURL = "http://59f015e4.ngrok.io/api/LipaMpesaValidationback",
                ValidationURL = "http://59f015e4.ngrok.io/api/LipaMpesaValidationback",
            };
            
            //await mpesasd.TryPostToMpesa("https://sandbox.safaricom.co.ke/mpesa/c2b/v1/registerurl",registerurl);
        }

        public static void c2bpaymentspost()
        {
            
        }

        public static async void MakePayment()
        {
           //anonymous object
            var c2b = new
            {
                CommandID = "CustomerPayBillOnline",
                Amount = "20",
                Msisdn = "254708374149",
                BillRefNumber = "72087",
                ShortCode = "603083"
            };
           // var mpesasd = new MpesaRepository();
           // await  mpesasd.TryPostToMpesa("https://sandbox.safaricom.co.ke/mpesa/c2b/v1/simulate",c2b);
        }
    }
    public class regurl
    {
        public string ShortCode { get; set; } = "600438";
        public string ResponseType { get; set; } = "Cancelled";
        public string ConfirmationURL { get; set; } = "http://42928a91.ngrok.io/api/paymentURL";
        public string ValidationURL { get; set; } = "http://42928a91.ngrok.io/api/paymentURL";
    }
    public class c2brequest
    {
        public string ShortCode { get; set; } = "600438";
        public string CommandID { get; set; } = "CustomerPayBillOnline";
        public string Amount { get; set; } = "500";
        public string BillRefNumber { get; set; } = "account";
    }
    
}
