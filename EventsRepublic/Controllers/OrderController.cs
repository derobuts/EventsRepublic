using EventsRepublic.Attributes;
using EventsRepublic.Models;
using EventsRepublic.Repository;
using EventsRepublic.Serializers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace EventsRepublic.Controllers
{
    [Route("api/Order")]
    public class OrderController : Controller
    {
        // GET: api/Order
        [ServiceFilter(typeof(CustomAuthorizeFilter))]
        [HttpGet]
        public async Task<int> GetAsync()
        {
            OrderRespository confirmorder = new OrderRespository();
            // var k = await confirmorder.ConfirmOrder(i);
            return 0;
        }

        // POST: api/Order
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]object value)
        {
            OrderRespository orderRespository = new OrderRespository();
            DateTime recurrencekey = DateTime.Now;
            JsonSerializerDeserializer jsonSerializerDeserializer = new JsonSerializerDeserializer();
            OrdertoBeReserved ordertobereserved = jsonSerializerDeserializer.ToObject<OrdertoBeReserved>(value.ToString());
            EventRespositoryv2 eventRespositoryv2 = new EventRespositoryv2();
            UserRespirotory userrp = new UserRespirotory();
            
            UserInfo user = new UserInfo();
            user.Email = "n/a";
            var Guestuserid = await userrp.AddGuestCheckoutUser(user);
           
            Payload payload = new Payload();
            payload.Role = 3;
            payload.Isregistered = false;
            payload.Iat = DateTimeOffset.Now.ToUnixTimeSeconds();
            payload.Expiry = DateTimeOffset.Now.AddMinutes(15).ToUnixTimeSeconds();
            payload.UserId = Guestuserid;

            if (ordertobereserved.Recurring)
            {
                recurrencekey = ordertobereserved.OrderDate.ToUniversalTime();
            }
            var ordercreated= await orderRespository.CreatOrder(ordertobereserved.Eventid, Guestuserid, ordertobereserved.TicketsToReserve,ordertobereserved.Recurring,recurrencekey,ordertobereserved.NoofTicketsInOrder);
            var chkouttoken = JsonWebToken.GetToken(payload);
            return Ok(JsonConvert.SerializeObject(new { token = chkouttoken, orderReserved = ordercreated }));
        }
        //confirm payment
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]object value)
        {
            OrderRespository confirmorder = new OrderRespository();
            var _orderconfirm = JsonConvert.DeserializeObject<OrderToPay>(value.ToString());
             //var resultcode = confirmorder.ConfirmOrder(_orderconfirm.orderid, _orderconfirm.amount,_orderconfirm.eventid);           
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }

}
