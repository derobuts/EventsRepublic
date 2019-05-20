using EventsRepublic.Attributes;
using EventsRepublic.Data;
using EventsRepublic.InterFace;
using EventsRepublic.Models;
using EventsRepublic.Repository;
using EventsRepublic.Serializers;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EventsRepublic.Controllers
{ 
    [Route("api/Order")]
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IOrderRepository _orderRepository;
        private readonly Microsoft.AspNetCore.Identity.UserManager<AppUser> _userManager;
        public OrderController(Microsoft.AspNetCore.Identity.UserManager<AppUser> userManager,IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
            _userManager = userManager;
        }

        // POST: api/Order
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CreateOrder([FromBody]OrdertoBeReserved ordertoBeReserved)
        {                                  
            if (ModelState.IsValid)
            {
                var h = _orderRepository;
                AppUser appUser = new AppUser();
                bool isAuthenticated = User.Identity.IsAuthenticated;
                if (!isAuthenticated)
                {
                    appUser = new AppUser { UserName = Guid.NewGuid().ToString(), SecurityStamp = Guid.NewGuid().ToString() };
                    var result = await _userManager.CreateAsync(appUser);
                    if (result.Succeeded)
                    {
                        Claim newClaim = new Claim(ClaimTypes.Role, "Customer");
                        await _userManager.AddClaimAsync(appUser, newClaim);
                    }
                }

                OrderRespository orderRespository = new OrderRespository();
                DateTime recurrencekey = DateTime.Now;
                if (ordertoBeReserved.Recurring)
                {
                    recurrencekey = ordertoBeReserved.OrderStartDate.ToUniversalTime();
                }
                await orderRespository.CreatOrder(ordertoBeReserved.Eventid, appUser.Id, ordertoBeReserved.TicketsToReserve, ordertoBeReserved.Recurring, recurrencekey, ordertoBeReserved.NoofTicketsInOrder, ordertoBeReserved.OrderStartDate.ToUniversalTime(), ordertoBeReserved.OrderEndDate.ToUniversalTime());
                return Accepted();
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
