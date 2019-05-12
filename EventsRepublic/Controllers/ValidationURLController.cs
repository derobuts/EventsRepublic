using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using EventsRepublic.Database;
using EventsRepublic.Models;
using EventsRepublic.Models.Mpesa;
using EventsRepublic.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static EventsRepublic.Models.Mpesa.MPESA;

namespace EventsRepublic.Controllers
{
    [Route("api/paymentURL")]
    public class ValidationURLController : Controller
    {
        // GET: api/ValidationURL
        [HttpGet]
        public async Task<string> GetAsync()
        {
          


            return "";
            //g.Defaultreqheaders;
        }
        // GET: api/ValidationURL/5
        [HttpGet("{id}", Name = "Gets")]
        public void Get(string id)
        {
         
        }

        // POST: api/ValidationURL
        [HttpPost]
        public async Task<string> Post([FromBody]object value)
        {
            try
            {
                OrderRespository orderrespository = new OrderRespository();
                var consumer2business = JsonConvert.DeserializeObject<c2bvalidationclass>(value.ToString());
                int resultcode = await orderrespository.ValidateOrderC2B(consumer2business.BillRefNumber, consumer2business.TransAmount);
                var mpesaresult = new
                {
                    ResultCode = resultcode != 0 ? 1:0,
                    ResultDesc = resultcode == 0 ? "Accepted" : "Rejected"
                };
                var k = JsonConvert.SerializeObject(mpesaresult);
                return k;
            }
            catch (Exception ex)
            {

                throw;
            }   
        }
    
        // PUT: api/ValidationURL/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {

        }
        
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {

        }
        public MpesaClassValidation MpesaValUrl(object value)
        {
            var k = value;
            MpesaClassValidation valclass = new MpesaClassValidation();
            var h = JsonConvert.DeserializeObject<UserInfo>(value.ToString());
            var data = (JObject)JsonConvert.DeserializeObject(value.ToString());
            valclass.MSISDN = data["MSISDN"].Value<string>();
            valclass.BillRef = data["BillRefNumber"].Value<int>();
            valclass.TransAmount = data["TransAmount"].Value<decimal>();
            return valclass;

        }
        public bool IsOrderSuccess(MpesaClassValidation b)
        {
            //send to db
            return true;
        }
    }
}
