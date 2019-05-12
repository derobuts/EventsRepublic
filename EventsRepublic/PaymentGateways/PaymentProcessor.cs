using EventsRepublic.InterFace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventsRepublic.Models.Mpesa
{
    public class PaymentProcessor
    {
        IPaymentGateway gateway = null;
        public void MakePayment(PaymentMethod method, Order product,PaymentPayload paymentpayload)
        {
            PaymentGatewayFactory factory = new PaymentGatewayFactory();
            this.gateway = factory.CreatePaymentGateway(method);

            this.gateway.MakePayment(product,paymentpayload);
        }
    }
}
