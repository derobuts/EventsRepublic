using EventsRepublic.InterFace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventsRepublic.Models.Mpesa
{
    public class BusinessToBusiness 
    {
        private static string url { get; set; } = " https://sandbox.safaricom.co.ke/mpesa/b2b/v1/paymentrequest";
        string securitycredential = "E3OtkpNWpAEneKo6qgxL0B8wCR04CtThlV1LFyqMlU7rCf8U1aul9jius9CrbQ29UuOVwrBSgZ+tOu1NbPU1n+mqhFWiYW3ZWw+Et+dXwbccn07IEE9Vy9C/yo2Qe++poHUysbKiNyiKgCCocq4tVN1JiaxvohHVQADEgIYdsVtdgtqfi+I7ZdMO9OQckG9TEWzv0oZbT39/8sOR+f+3MVV11xdU68HkmwUIliXiRracENIFaxgEtBVt+zRK0zXh5A6sxndysfyVbl6kt+1o26OGH+aTf0oavmCU+BsGEAmEg3PRXx1w2bG9fEW1Fe+aIqevS0HpdPLAynfXGH1hvQ==";
        public void MakePayment(OrderToPay order)
        {
           
        }
    }
}
