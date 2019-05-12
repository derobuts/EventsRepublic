using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using EventsRepublic.Database;
using EventsRepublic.Models;
using EventsRepublic.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventsRepublic.Controllers
{
    [Produces("application/json")]
    [Route("api/PaymentGateway")]
    public class STKPushPaymentGatewayController : Controller
    {
        // GET: api/PaymentGateway
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/PaymentGateway/5
        [HttpGet("{id}", Name = "Geto")]
        public string Get(int id)
        {
            return "value";
        }
        
        // POST: api/PaymentGateway
        [HttpPost]
        public async Task<int> Post([FromBody]string value)
        {
            SqlParameter amount = new SqlParameter("@amount", DbType.Decimal);
            amount.Value = 170;
            SqlParameter prm = new SqlParameter("@OrderRefNum", DbType.Int32);
            prm.Value = 690;
            DBContext b = new DBContext(new DBCOntext(), "OrderValidity", amount, prm);
            return 0;
        }
        
        // PUT: api/PaymentGateway/5
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
