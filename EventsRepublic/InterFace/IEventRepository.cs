using EventsRepublic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventsRepublic.InterFace
{
    public interface IEventRepository : IRepository<Event>
    {
        IEnumerable<Event> GetEventsByCategory(decimal latitude, decimal longitude, int pageno, int pagesize, int categoryid, int milestosearch);
    }
}
