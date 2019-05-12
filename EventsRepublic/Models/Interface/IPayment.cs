using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventsRepublic.Models.Interface
{
    interface IPayment
    {
        bool Pay(decimal amount);
    }
}
