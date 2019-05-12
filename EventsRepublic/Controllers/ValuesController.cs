using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventsRepublic.Attributes;
using EventsRepublic.Database;
using EventsRepublic.Models;
using EventsRepublic.Models.Mpesa;
using EventsRepublic.NexmoSms;
using EventsRepublic.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EventsRepublic.Controllers
{
    [Route("api/values")]

    public class ValuesController : Controller
    {
        // GET api/values
        
        [HttpGet]
       // [CustomAuthorizeAttribute]
        public async Task<string> Get()
        {

          //  ConsumerToBusiness.Registerc2burl();
            
           // ConsumerToBusiness.MakePayment();

           // Nexmosms sms = new Nexmosms();
           // sms.SendSms();

           var hu = new JwtToken.Header() { alg = "HS256",typ="JWT" };
            //RSAClass rSAClass;
            //RSAClass.loadsslx509("DEROBUTS");
            
           Payload payload = new Payload();
            payload.Email = "t";
            payload.Expiry = DateTimeOffset.UtcNow.AddMinutes(30).ToUnixTimeSeconds();
            payload.Role = 2 ;
            payload.UserId = 34;
           // var ku = new JwtToken.Body() {Iss="E",exp="E",App_ID=Guid.NewGuid(),Role="G",User_Id=1,idp="Derobuts" };
           var token = JsonConvert.SerializeObject(JsonWebToken.GetToken(payload));
            return token;
        }
  
        // GET api/values/5
        [HttpGet("{id}")]
        [EnableCors("CorsPolicy")]
        public async Task<string> Get(string id)
        {
            
            SqlParameter prm = new SqlParameter("@name", DbType.String);
            prm.Value = id;
            //GodObject<object> b = new GodObject<object>(new DBCOntext(), "GetTicketLike", prm);
            //var k = await b.GetAsync();
            //var json = JsonConvert.SerializeObject(k);
            return "";
       }

        // POST api/values
        [HttpPost]
        public string Post([FromBody]string value)
        {
            var k = JsonWebToken.ValidateToken(value,true);
            return k.ToString();
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
