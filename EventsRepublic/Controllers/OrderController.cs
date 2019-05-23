using EventsRepublic.Attributes;
using EventsRepublic.Data;
using EventsRepublic.InterFace;
using EventsRepublic.Models;
using EventsRepublic.Repository;
using EventsRepublic.Serializers;
using EventsRepublic.Service;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EventsRepublic.Controllers
{ 
    [Route("api/Order")]
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly Microsoft.AspNetCore.Identity.UserManager<AppUser> _userManager;
        public OrderController(Microsoft.AspNetCore.Identity.UserManager<AppUser> userManager,IOrderRepository orderRepository,IJwtTokenService jwtTokenService)
        {
            _orderRepository = orderRepository;
            _userManager = userManager;
            _jwtTokenService = jwtTokenService;
        }

        // POST: api/Order
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CreateOrder([FromBody]OrdertoBeReserved ordertoBeReserved)
        {                                  
            if (ModelState.IsValid)
            {
                try
                {                                    
                    var appUser = new AppUser { UserName = Guid.NewGuid().ToString(), SecurityStamp = Guid.NewGuid().ToString(), Email = "Guest@eventdb" };
                    var result = await _userManager.CreateAsync(appUser);                   
                    if (result.Succeeded)
                    {
                        List<Claim> Claims = new List<Claim>() { new Claim(ClaimTypes.Role, "GuestCustomer")};                        
                        await _userManager.AddClaimsAsync(appUser,Claims);
                    }
                    else
                    {
                        return NotFound();
                    }                          
                    int Orderid = await _orderRepository.CreatOrder(ordertoBeReserved.Eventid, appUser.Id, ordertoBeReserved.TicketsToReserve, ordertoBeReserved.Recurring, ordertoBeReserved.NoofTicketsInOrder, ordertoBeReserved.OrderStartDate.ToUniversalTime(), ordertoBeReserved.OrderEndDate.ToUniversalTime());
                    return Ok(new {token = _jwtTokenService.CreateToken(appUser),orderid = Orderid});
                }
                catch (Exception ex)
                {

                    throw;
                }
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
        [HttpGet()]
        public async Task<IActionResult>UserOrder(int orderid)
        {
            if (ModelState.IsValid)
            {
                    var currentuserid = User.Identity.GetUserId<int>();
                    var Ordersummary = await _orderRepository.GetUserOrder(orderid,currentuserid);
                    Ordersummary.Reserveditems = await _orderRepository.GetItemsinOrder(orderid);
                    return Ok(Ordersummary);
            }    
            return NotFound();
        }
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }

}
