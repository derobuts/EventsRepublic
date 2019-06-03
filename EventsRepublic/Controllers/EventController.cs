using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using EventsRepublic.Attributes;
using EventsRepublic.Database;
using EventsRepublic.Models;
using EventsRepublic.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace EventsRepublic.Controllers
{
    public class EventController : Controller
    {       
        [HttpGet("api/eventsnearby")]
      
        public async Task<IActionResult> EventsNearby(decimal latitude, decimal longitude)
        {
            var getEventsNearby = new EventRespositoryv2();
            var homepageEvents = await getEventsNearby.PopularNearby(latitude, longitude);
            return Ok(JsonConvert.SerializeObject( homepageEvents)); 
        }

        [HttpGet("api/geteventdetail")]
        [AllowAnonymous]
        public async Task<string> Get(Guid eventid)
        {
            var getEvent = new EventRespositoryv2();

            var Event = await getEvent.GeteventDetails(eventid);

            return JsonConvert.SerializeObject(Event);
        }
        [HttpGet("api/getuserevents")]
        public async Task<IActionResult>GetUserEvents(int eventstatus)
        {
            object User;
            HttpContext.Items.TryGetValue("principaluser", out User);
            var Userinfo = (Payload)User;
            var eventRespositoryv2 = new EventRespositoryv2();
            var userevents = await eventRespositoryv2.GetUserEvents(Userinfo.UserId, eventstatus);
            return new ObjectResult(userevents);
        }

        [HttpGet("api/eventcategories")]
        public async Task<IActionResult> EventCategory()
        {
            var EventCategories = await new EventRespositoryv2().EventCategories();
            return Ok(JsonConvert.SerializeObject(new { eventcategories = EventCategories }));
        }
        //
        // POST: api/Event
        [HttpPost("api/createevent")]
        public async Task Post([FromBody]Event value)
        {
            try
            {
                //value.user_id = 1;//temporary
                var eventrespository = new EventRespositoryv2();
                eventrespository.AddEvent(value);
            }
            catch (Exception ex)
            {
                var h = ex;
                throw;
            }
           
        }  
        // PUT: api/Event/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
         
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public int Delete(int id)
        {
            var deleteEvent = new EventRespisotory();
            return deleteEvent.deleteEventAsync(id);
        }
    }
}
