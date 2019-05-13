using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventsRepublic.Database;
using EventsRepublic.Models;
using EventsRepublic.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace EventsRepublic.Controllers
{

    [Route("api/Search")]
    public class SearchController : Controller
    {
        // GET: api/Search
        //search all Events,Venues,Perfomer with a given Name
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return default;
        }

        // GET: api/Search/5
        [HttpGet]
        [Route("search")]
        /**will add proper paging later**/
        public async Task<IActionResult>Get([FromQuery]string query,[FromQuery]string searchtype)
        {
            switch (searchtype)
            {
                case "event":
                    return new ObjectResult(await new EventRespositoryv2().GeteventByName<EventSubinfo>(query));
                case "venue":
                    return new ObjectResult(await new VenueRepository().GetVenueByName<VenueSearch>(query,0,7));
                case "performer":
                    return new ObjectResult(await new PerformerRepository().GetPerformerbyname<Performer>(query,0,7));

            }
            return new ObjectResult("not found");
        }
        
        // POST: api/Search
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }
        
        // PUT: api/Search/5
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
