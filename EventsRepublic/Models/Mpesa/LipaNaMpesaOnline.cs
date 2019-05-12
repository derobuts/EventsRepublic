using Dapper;
using EventsRepublic.Database;
using EventsRepublic.InterFace;
using EventsRepublic.Repository;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace EventsRepublic.Models
{
    public class LipaNaMpesaOnline : IPaymentGateway
    {
        string requesturl = "https://sandbox.safaricom.co.ke/mpesa/stkpush/v1/processrequest";
        int retry = 0;
       
        public LipaNaMpesaOnline()
        {

        }

        public async void MakePayment(object Order)
        {
           
        }


        public async void MakePayment(Order value,PaymentPayload paymentpayload)
        {
            var lipaMpesaPayload= JsonConvert.DeserializeObject<LipaMpesaPayload>(paymentpayload.Payload.ToString());
           // var order = (LipaMpesaPayload)paymentpayload.Paymentpayload;
            var LMO = new
            {
                BusinessShortCode = "174379",
                PartyA = lipaMpesaPayload.Msisdn.Remove(0, 1),
                PartyB = "174379",
                TransactionType = "CustomerPayBillOnline",
                Amount = value.Amount,
                AccountReference = "ref",
                TransactionDesc = "work hard",
                CallBackURL = "http://9c50ab8b.ngrok.io/api/LipaMpesaOnlineCallback",
                Timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss"),
                Password = MpesaFactory.LipaMpesaOnlinePassKey(),
                PhoneNumber = "254703567954"
            };
            MpesaFactory mpesaFactory = new MpesaFactory();
            var response = await mpesaFactory.TryPostToMpesa(requesturl, LMO);
            if (response.Item1)
            {
                var lmoresponse = JsonConvert.DeserializeObject<LMOResponse>(response.Item3);
                string sql = $"update OrderTransaction set Merchant_ref = @Merchant_ref where OrdersId = @OrdersId";


                using (var con = new SqlConnection(new DBCOntext().GetdbConnectionString()))
                {
                    //var sqlparams = new DynamicParameters();
                    await con.ExecuteAsync(sql, new { Merchant_ref = lmoresponse.MerchantRequestID, OrdersId = 1 });
                }
                // sqlparams.Add("@user_id",UserId, DbType.String);               
            }
        }
    }
    public class LMOResponse
    {
        public string MerchantRequestID { get; set; }
        public string CheckoutRequestID { get; set; }
    }
    public class StkCallback
    {
        public string MerchantRequestID { get; set; }
        public string CheckoutRequestID { get; set; }
        public int ResultCode { get; set; }
        public string ResultDesc { get; set; }
        public CallbackMetadata CallbackMetadata { get; set; }
    }
    public class Item
    {
        public string Name { get; set; }
        public object Value { get; set; }
    }

    public class CallbackMetadata
    {
        public List<Item> Item { get; set; }
    }
    public class Body
    {
        public StkCallback stkCallback { get; set; }
    }

    public class RootObject
    {
        public Body Body { get; set; }
    }
    public class LipaMpesaPayload
    {
        public string Msisdn { get; set; }
    }

}
