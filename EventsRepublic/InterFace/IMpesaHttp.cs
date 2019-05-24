using System.Net.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventsRepublic.Models;

namespace EventsRepublic.InterFace
{
    public interface IMpesa
    {
         void LipaMpesaOnlineStk(LipaMpesaStkPayload lipaMpesaStkPayload);
    }
}
