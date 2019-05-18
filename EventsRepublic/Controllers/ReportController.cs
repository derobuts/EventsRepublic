using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventsRepublic.Attributes;
using EventsRepublic.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace EventsRepublic.Controllers
{
    public class ReportController : Controller
    {
        // GET: api/Report
        
        public IEnumerable<string> GetOrderReportsByEvent(int eventid)
        {

            return new string[] { "value1", "value2" };
        }
        [HttpGet("api/getorderreports")]
        public async Task<IActionResult> GetOrderReports(int eventid,DateTime startdate,DateTime enddate)
        {
            object User;
            HttpContext.Items.TryGetValue("principaluser", out User);
            var Userinfo = (Payload)User;
            var Report = new Reports();
            var OrderReport = eventid == 0 ? await Report.GetOrderReports(Userinfo.UserId, startdate, enddate) : await Report.GetOrderReportsByEvent(Userinfo.UserId,startdate,  enddate, eventid);
            return Ok(JsonConvert.SerializeObject(OrderReport));
        }
        [HttpGet("api/ticketreports")]
        public IEnumerable<string> GetTicketReports(int eventid, DateTime startdate, DateTime enddate)
        {
            object User;
            HttpContext.Items.TryGetValue("principaluser", out User);
            var Userinfo = (Payload)User;
            return new string[] { "value1", "value2" };
        }
        [HttpGet("api/accounttxreports")]
        public async Task<IActionResult> GetAccounTransactionReport(DateTime startdate, DateTime enddate)
        {
            object User;
            HttpContext.Items.TryGetValue("principaluser", out User);
            var Userinfo = (Payload)User;
            var Report = new Reports();
            var Txreport =await Report.GetTransactionHistory(Userinfo.UserId, startdate, enddate);
            return Ok(JsonConvert.SerializeObject(Txreport));
        }
        
    }
}
 