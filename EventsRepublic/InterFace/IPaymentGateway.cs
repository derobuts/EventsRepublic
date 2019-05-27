using EventsRepublic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventsRepublic.InterFace
{
   public interface IPaymentGateway
    {
        void MakePayment(T paymentpayload);
    }

    public class PaymentPayload
    {
        public int Paymentmethod { get; set; }
        public object Payload { get; set; }
    }
    public class PaymentPayloadRootObject
    {
        public PaymentPayload PaymentPayload { get; set; }
    }
}
