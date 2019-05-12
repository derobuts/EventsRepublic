using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using EventsRepublic.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace EventsRepublic.Controllers
{ 
    [Route("api/Token")]
    public class TokenController : Controller
    {
        // GET: api/Token
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Token/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Token
        //refresh token
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody]string value)
        {
            JsonWebToken jwt = new JsonWebToken();
            var Tokens = JsonConvert.DeserializeObject<Tokens>(value);
            var newTokensTuple = await  jwt.TokenRenew(Tokens.RefreshToken, Tokens.jwtToken);
            if (newTokensTuple.Item1 == true)
            {
                return new ObjectResult(new { tokens = newTokensTuple.Item2 });
            }
            return new RedirectResult("login");
        }
        // PUT: api/Token/5
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
