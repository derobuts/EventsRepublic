using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using Dapper;
using EventsRepublic.Attributes;
using EventsRepublic.ClickSendApiall;
using EventsRepublic.Database;
using EventsRepublic.InterFace;
using EventsRepublic.Models;
using EventsRepublic.Models.Mpesa;
using EventsRepublic.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace EventsRepublic.Controllers
{  
    public class PaymentController : Controller
    {
        // GET: api/Payment
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Payment/5
        [HttpGet("api/getpayments")]
        public string Get1(int id)
        {
            return "value";
        }
        [HttpGet("api/myvalues")]
        public string GetO(int id)
        {
            return "value";
        }

        // POST: api/Payment
        [HttpPost("api/PostPayment")]
        public async Task<ActionResult> Post([FromBody]object value)
        {
            
            object User;
            HttpContext.Items.TryGetValue("principaluser", out User);
            var Userinfo = (Payload)User;
            var paymentpayload = JsonConvert.DeserializeObject<PaymentPayloadRootObject>(value.ToString());
            //
            OrderRespository orderRespository = new OrderRespository();
            //var getuserorder = await orderRespository.GetUserOrder(Userinfo.UserId);

            PaymentProcessor paymentProcessor = new PaymentProcessor();
           // paymentProcessor.MakePayment((PaymentMethod)paymentpayload.PaymentPayload.Paymentmethod, getuserorder,paymentpayload.PaymentPayload);
            return Ok();
        }
        //
        [HttpPost("api/LipaMpesaValidationback")]
        public string c2bvalidation([FromBody]object value)
        {
            var LipaMpesac2b = JsonConvert.DeserializeObject<ConsumerToBusiness>(value.ToString());

            try
            {
                using (var con = new SqlConnection(new DBCOntext().GetdbConnectionString()))
                {
                    var order = con.Query<dynamic>(@"select ISNULL((SELECT CASE
                                  WHEN Status = 101 THEN 'order has already been paid'
                                  WHEN DATEDIFF(MINUTE,GETUTCDATE(),Expiryts) < 0 THEN 'order has expired try again'
								  WHEN Amount < @amount THEN 103 THEN 'Amount is less than requested'
								  WHEN Amount  > @amount THEN 104 THEN 'Amount more than requested'
								  when Amount = @amount Then 1 'Accepted'
								  END
								  FROM OrderTransaction WHERE MobileRef_Num = @mobilerefnum),'order not found')", new { @mobilerefnum = LipaMpesac2b.BillRefNumber}).FirstOrDefault();
                    if (order != null)
                    {
                        if (order == "Accepted")
                        {
                            return JsonConvert.SerializeObject(new { ResultCode = 0, ResultDesc = order });
                        }
                        return JsonConvert.SerializeObject(new { ResultCode = 1, ResultDesc = order });
                    }
                    return JsonConvert.SerializeObject(new { ResultCode = 1, ResultDesc = order });
                }
            }
            catch (Exception ex)
            {

                throw;
            }
           
        }


        // PUT: api/Payment/5
        [HttpPost("api/LipaMpesaCallback")]
        public async void c2bLipanaMpesa([FromBody]string value)
        {
            try
            {
                var LipaMpesac2b = JsonConvert.DeserializeObject<ConsumerToBusiness>(value.ToString());
                
                    using (var con = new SqlConnection(new DBCOntext().GetdbConnectionString()))
                    {

                    var order = await con.Query<dynamic>(@"SELECT OrdersId FROM OrderTransaction  WHERE MobileRef_Num = @mobilerefnum",new { @mobilerefnum = LipaMpesac2b.BillRefNumber}).FirstOrDefault();
                    var sqlparams = new DynamicParameters();
                    sqlparams.Add("@orderid",order);
                    sqlparams.Add("@amount",LipaMpesac2b.TransAmount);
                    sqlparams.Add("@paymentmethod", 101);
                    sqlparams.Add("@txdate", DateTime.UtcNow);
                    sqlparams.Add("@txcode",LipaMpesac2b.TransID);
                    sqlparams.Add("@txoperation", 100);

                    await con.ExecuteAsync("ConfirmOrder", sqlparams, commandType: CommandType.StoredProcedure);               
                    }
                
            }
            catch (Exception ex)
            {

                throw;
            }

        }
        [HttpPost("api/LipaMpesaOnlineCallback")]
        public async void LmoCallback([FromBody]object value)
        {
            try
            {
                var LMOCallback = JsonConvert.DeserializeObject<RootObject>(value.ToString());
                if (LMOCallback.Body.stkCallback.ResultCode == 0)
                {
                    using (var con = new SqlConnection(new DBCOntext().GetdbConnectionString()))
                    {
                        var order = con.Query<dynamic>(@"SELECT E.User_Id,OT.NetAmount,OT.Amount,OT.OrdersId FROM Event E
                        INNER JOIN OrderTransaction OT on OT.Event_Id = E.Event_Id WHERE Merchant_ref  = @merchant_ref", new { @merchant_ref = LMOCallback.Body.stkCallback.MerchantRequestID }).FirstOrDefault();
                        var sqlparams = new DynamicParameters();
                        sqlparams.Add("@orderid",order.OrdersId, DbType.Int32);
                        sqlparams.Add("@txcode", LMOCallback.Body.stkCallback.CallbackMetadata.Item[1].Value,DbType.AnsiStringFixedLength);
                        sqlparams.Add("@netamount", LMOCallback.Body.stkCallback.CallbackMetadata.Item[0].Value,DbType.Decimal);
                        sqlparams.Add("@txoperation", 100,DbType.Int32);
                        sqlparams.Add("@paymentmethod", 100,DbType.Int32);
                       // sqlparams.Add("@txdate",DateTime.UtcNow);           
                        await con.ExecuteAsync("ConfirmOrder", sqlparams, commandType: CommandType.StoredProcedure);
                        OrderRespository orderRespository = new OrderRespository();
                        orderRespository.SendOrderEmail(order);
                    }
                }
                OrderRespository orderRespository2 = new OrderRespository();
                orderRespository2.SendOrderEmail(16);
                Clicksendapis clickSend = new Clicksendapis();
                //clickSend.SendSms();
            }
            catch (Exception ex)
            {

                throw;
            }           
            
        }

        [HttpPost]
        [ActionName("ConsumerToBusinessCallback")]
        public void C2BCallback([FromBody]object value)
        {

        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
