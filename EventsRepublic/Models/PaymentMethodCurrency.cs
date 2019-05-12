using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventsRepublic.Models
{
    public class PaymentMethodCurrency
    {
        int PaymentMethod;
        char CurrencyCode;

        public int PaymentMethod1 { get => PaymentMethod; set => PaymentMethod = value; }
        public char CurrencyCode1 { get => CurrencyCode; set => CurrencyCode = value; }
    }
}
