using EventsRepublic.Attributes;
using EventsRepublic.Models;
using EventsRepublic.Repository;
using EventsRepublic.Serializers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace EventsRepublic.Controllers
{
    [Authorize]
    [Route("api/Order")]
    public class OrderController : Controller
    {
        // GET: api/Orde
        [HttpGet]
        public async Task<int> GetAsync()
        {
            OrderRespository confirmorder = new OrderRespository();
            // var k = await confirmorder.ConfirmOrder(i);
            return 0;
        }

        // POST: api/Order
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]OrdertoBeReserved ordertoBeReserved)
        {
            if (ModelState.IsValid)
            {
                OrderRespository orderRespository = new OrderRespository();
                DateTime recurrencekey = DateTime.Now;         
                if (ordertoBeReserved.Recurring)
                {
                    recurrencekey = ordertoBeReserved.OrderStartDate.ToUniversalTime();
                }
                var ordercreated = await orderRespository.CreatOrder(ordertoBeReserved.Eventid, 5, ordertoBeReserved.TicketsToReserve, ordertoBeReserved.Recurring, recurrencekey, ordertoBeReserved.NoofTicketsInOrder, ordertoBeReserved.OrderStartDate.ToUniversalTime(), ordertoBeReserved.OrderEndDate.ToUniversalTime());
                var chkouttoken = JsonWebToken.GetToken(new { });
                return Ok(JsonConvert.SerializeObject(new { token = chkouttoken, orderReserved = ordercreated }));
            }
            return NotFound();
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
