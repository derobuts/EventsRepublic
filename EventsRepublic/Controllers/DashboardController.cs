using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventsRepublic.Attributes;
using EventsRepublic.Models;
using EventsRepublic.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace EventsRepublic.Controllers
{
    [ServiceFilter(typeof(CustomAuthorizeFilter))]
    public class DashboardController : Controller
    {
        // GET: api/Dashboard/5
        [HttpGet("api/dashboard")]
        public async Task<string> Get(DateTime startdate,DateTime enddate,int eventid)
        {
            object User;
            HttpContext.Items.TryGetValue("principaluser", out User);      
            var Userinfo = (Payload)User;
            DashboardRepository dashboardRepository = new DashboardRepository();
            EventRespositoryv2 eventRespositoryv2 = new EventRespositoryv2();
            OrderRespository orderRespository = new OrderRespository();
            //var UserEvents = await eventRespositoryv2.GetUserEvents(Userinfo.UserId);
            var MonthlystatsTask = eventRespositoryv2.GetMonthlyStats(eventid, startdate.ToUniversalTime(), enddate.ToUniversalTime());
            //var getGrossnet = eventid == 0 ?  dashboardRepository.GrossNet(Userinfo.UserId) :  dashboardRepository.GrossNetPerEvent(Userinfo.UserId,eventid);
            var aggregateOrderTicketSales = dashboardRepository.AggregateOrderticketsalesPerEvent(Userinfo.UserId, startdate.ToUniversalTime(), enddate.ToUniversalTime(), eventid);
            //var Balance = dashboardRepository.Balance(Userinfo.UserId);
           // var EventCategories = await new EventRespositoryv2().EventCategories();
            //var orderTicketsaleGrouped = eventid == 0 ?  dashboardRepository.OrderTicketSalegrouped(Userinfo.UserId, startdate.ToUniversalTime(), enddate.ToUniversalTime()) : dashboardRepository.OrdeTicketSalegroupedPerEvent(Userinfo.UserId, startdate.ToUniversalTime(), enddate.ToUniversalTime(), eventid);
            var LatestOrdersTask = orderRespository.GetLatestOrders();
            // alltasks.AddRange()
            try
            {
                await Task.WhenAll(LatestOrdersTask,MonthlystatsTask);
            }
            catch (Exception ex)
            {
                throw;
            }          
            return JsonConvert.SerializeObject(new { monthlystats = await MonthlystatsTask,latestorders = await LatestOrdersTask});
        }
        
        public string GetUserBalance(int userid)
        {
            UserRespirotory userRespirotory = new UserRespirotory();
            var balance = userRespirotory.GetUserBalance(userid);
            return balance;
        }

        // POST: api/Dashboard
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }
        
        // PUT: api/Dashboard/5
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
