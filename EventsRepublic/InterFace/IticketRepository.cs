using EventsRepublic.Models;
using EventsRepublic.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventsRepublic.InterFace
{
   public interface IticketRepository
    {
        Task<IEnumerable<TicketClassSubinfo>> GeteventTicketClass(Guid eventid);
        Task<IEnumerable<TicketClassSubinfo>> GetEventRecurringTicketClasses(Guid eventid, DateTime recurrencekey);
    }
}
