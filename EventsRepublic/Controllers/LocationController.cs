using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventsRepublic.Controllers
{
    [Route("api/Location")]
    public class LocationController : Controller
    {
        // GET: api/Location
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Location/5
        [HttpGet("{id}", Name = "Getg")]
        public string Get(int id)
        {
            return "value";
        }
        
        // POST: api/Location
        [HttpPost]
        public void Post([FromBody]object value)
        {

        }
        
        // PUT: api/Location/5
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
