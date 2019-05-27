using EventsRepublic.InterFace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventsRepublic.Models.Mpesa
{
    /**
     * This API enables Business to Customer (B2C) transactions between a company and customers 
     * Enable Payout to consumer maximum should be 70,000 per transaction 140,000 daily trans
     * **/
    public class BusinessToConsumer : IPaymentGateway
    {
        private static string url { get; set; } = "https://sandbox.safaricom.co.ke/mpesa/b2c/v1/paymentrequest";
        string securitycredential = "E3OtkpNWpAEneKo6qgxL0B8wCR04CtThlV1LFyqMlU7rCf8U1aul9jius9CrbQ29UuOVwrBSgZ+tOu1NbPU1n+mqhFWiYW3ZWw+Et+dXwbccn07IEE9Vy9C/yo2Qe++poHUysbKiNyiKgCCocq4tVN1JiaxvohHVQADEgIYdsVtdgtqfi+I7ZdMO9OQckG9TEWzv0oZbT39/8sOR+f+3MVV11xdU68HkmwUIliXiRracENIFaxgEtBVt+zRK0zXh5A6sxndysfyVbl6kt+1o26OGH+aTf0oavmCU+BsGEAmEg3PRXx1w2bG9fEW1Fe+aIqevS0HpdPLAynfXGH1hvQ==";
        //string securitycredential = RSAClass.loadsslx509("12qw");


        public async void MakePayment(object value)
        {
            
        }

        public async void MakePayment(Order value, PaymentPayload paymentpayload)
        {
            //var order = (OrderToPay)paymentpayload.Paymentpayload;
            var b2c = new
            {
                InitiatorName = "testapi",
                SecurityCredential = securitycredential,
                CommandID = "BusinessPayment",
                Amount = value.Amount,
                PartyA = "600438",
                PartyB = "der",
                Remarks = "Payment out",
                QueueTimeOutURL = "",
                ResultURL = "",
                Occassion = "payout"
            };
            //MpesaRepository mpesaFactory = new MpesaRepository();
            //var response = await mpesaFactory.TryPostToMpesa(url, b2c);
           
        }

        public void MakePayment(T paymentpayload)
        {
            throw new NotImplementedException();
        }
    }
}

