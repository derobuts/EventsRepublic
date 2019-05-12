using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventsRepublic.Database;
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
        [Route("earch")]
        public async Task<IActionResult> Get([FromQuery]string query)
        {
            var SearchRepository = await new SearchRepository().GetSearchQuery(query);
            return Ok(JsonConvert.SerializeObject(SearchRepository));
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
