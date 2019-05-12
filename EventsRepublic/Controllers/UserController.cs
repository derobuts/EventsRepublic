using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using EventsRepublic.Attributes;
using EventsRepublic.Models;
using EventsRepublic.Models.Mpesa;
using EventsRepublic.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EventsRepublic.Controllers
{
   
    public class UserController : Controller
    {
        // GET: api/Register
        [HttpGet]      
        public void GuestUserToken()
        {
            //ConsumerToBusiness.Registerc2burl();
            //ConsumerToBusiness.c2bpaymentspost();
           //var securitycred = RSAClass.loadsslx509(MpesaFactory.LipaMpesaOnlinePassKey());
           //BusinessToConsumer b2c = new BusinessToConsumer("testapi","BusinessPayment","1000","600438","254708374149","work", "http://b1fcbd51.ngrok.io/api/Register", "http://b1fcbd51.ngrok.io/api/Register", "work");
           //BusinessToConsumer.PostPayment(new MpesaFactory(), b2c);
           // RSAClass.loadsslx509("derobuts23");
        }
        // GET: api/Register/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            
            return "value";
        }
        // POST: api/RegisterUser
        [HttpPost("api/addguestuser")]
        public async Task<IActionResult> PostAsync([FromBody]object value)
        {
            UserRespirotory user = new UserRespirotory();
            var userinfo = JsonConvert.DeserializeObject<UserInfo>(value.ToString());
            var issuccess = await user.AddGuestCheckoutUser(userinfo);
            if (issuccess == 0)
            {
                return Ok();
            }
            
            return BadRequest(ModelState);
        }
        [HttpPost("api/Login")]
        public async Task<IActionResult> Login([FromBody]object value)
        {
            UserRespirotory user = new UserRespirotory();
            var userinfo = JsonConvert.DeserializeObject<UserInfo>(value.ToString());
            var issuccess = await user.Login(userinfo);
            if (issuccess.Item1)
            {
                return Ok(new {token =  issuccess.Item2 });
            }

            return StatusCode(401,issuccess.Item2);
        }

        [HttpPost("api/adduser")]
        public async Task<IActionResult> createUser([FromBody]object value)
        {
            UserRespirotory user = new UserRespirotory();
            var userinfo = JsonConvert.DeserializeObject<UserInfo>(value.ToString());
            var useraddedtuple = await user.AddUser(userinfo);
            if (!useraddedtuple.Item1)
            {
                if (useraddedtuple.Item2 == "Email exists")
                {
                    return StatusCode(409, "Email Exists");
                }
                if (useraddedtuple.Item2 == "error")
                {
                    return StatusCode(500, "Server Error");
                }
            }
            return Ok(useraddedtuple.Item2);
        }

        // PUT: api/Register/5
        //update user
        [HttpPut("api/updateguestuser")]
        [ServiceFilter(typeof(CustomAuthorizeFilter))]
        public async void Put([FromBody]object value)
        {
            object User;
            HttpContext.Items.TryGetValue("principaluser", out User);   
            var Userinfo = (Payload)User;
            UserRespirotory guestrepository = new UserRespirotory();
            var guestuserinfo = JsonConvert.DeserializeObject<UserInfo>(value.ToString());
            guestuserinfo.UserId = Userinfo.UserId;
            await guestrepository.UpdateGuestCheckoutUserInfo(guestuserinfo);
        }
        
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
