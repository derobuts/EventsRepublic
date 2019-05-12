using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventsRepublic.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventsRepublic.Controllers
{
 
    public class TransactionsController : Controller
    {
        // GET: api/Transactions
        [HttpGet("api/transactions")]
        public async Task<IEnumerable<Transactions>> GetTxAsync(int userid,DateTime startdate,DateTime enddate,int pageno,int pagesize)
        {
            Transactions tx = new Transactions();
            var transactions = await tx.GetTransactions(userid,startdate,enddate,pageno,pagesize);
            return transactions;
        }

        // GET: api/Transactions/5
        [HttpGet()]
        public string Get(int id)
        {
            return "value";
        }
        
        // POST: api/Transactions
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }
        
        // PUT: api/Transactions/5
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
