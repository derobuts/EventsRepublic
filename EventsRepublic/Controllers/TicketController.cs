using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventsRepublic.Models;
using EventsRepublic.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace EventsRepublic.Controllers
{ 
    //[Route("api/ticketgroups/")]
    public class TicketController : Controller
    {
        // GET: api/Ticket
        //[HttpGet]
        [HttpGet("api/ticketgroups")]
        public async Task<IActionResult> Get(Guid eventid)
        {
            var ticketRepository = new TicketRespository();
            var EventTicketClasses = await ticketRepository.GeteventTicketClass(eventid);
            return  Ok(JsonConvert.SerializeObject(EventTicketClasses));
        }

        [HttpGet("api/recurringticketgroups")]
        public async Task<IActionResult> GetReccurenceTickets(Guid eventid, DateTime recurrenceKey)
        {
            var ticketRepository = new TicketRespository();
            var EventTicketClasses = await ticketRepository.GetEventRecurringTicketClasses(eventid, recurrenceKey.ToUniversalTime());
            return Ok(JsonConvert.SerializeObject(EventTicketClasses));
        }

        [HttpGet("{id}", Name = "Geteventtickclasses")]
        public async Task<string> GetEventTicketClasses(int eventid,int eventshowingdateid)
        {
            return "";
        }
        
        [HttpPost]
        public async Task<string> Post([FromBody]object value)
        {
            var ordertoBeReserved = JsonConvert.DeserializeObject<OrdertoBeReserved>(value.ToString());
            TicketRespository ticketRespository = new TicketRespository();
            var isticketreserved = await ticketRespository.ReserveTickets(ordertoBeReserved.Eventid, ordertoBeReserved.TicketsToReserve);
           // var result = await orderRespository.CreatOrder(JsonConvert.DeserializeObject<CreateOrder[]>(value.ToString()));
            return "";
        }
        
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }       
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
