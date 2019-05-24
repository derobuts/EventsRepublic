
using Dapper;
using EventsRepublic.Database;
using EventsRepublic.InterFace;
using EventsRepublic.Models.Mpesa;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
namespace EventsRepublic.Models
{
    public class MpesaRepository :IMpesa
    {
        static string app_key = "GE1LxhSyh5xAbOEta7rtpapi6Gl2zhM4";
        static string app_secret = "3F4I1OG7IoxZO3kD";        
        private readonly IHttpClientFactory _httpClientFactory;
        static string lipampesa = "https://sandbox.safaricom.co.ke/mpesa/stkpush/v1/processrequest";
        static string LipaNaMpesashortcode = "174379";
        static string passkey = "bfb279f9aa9bdbcf158e97dd71a467cd2e0c893059b10f78e6b72ada1ed2c919";
        //static string mpesaurl = "https://sandbox.safaricom.co.ke/mpesa/stkpush/v1/processrequest";
        //private static Token Defaulttoken = null;
        //private static TimeSpan ts;
        private static Token MpesaToken = null;

        public MpesaRepository(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        private async Task<Token> GetToken()
        {

         MpesaToken =  MpesaToken == null || (MpesaToken.ExpiresIn - DateTime.UtcNow).Minutes < 2 ? await GetAyncToken() : MpesaToken;
         return MpesaToken;
        }

        private async Task<Token> GetAyncToken()
        {           
            string appKeySecret = app_key + ":" + app_secret;
            byte[] bytes = Encoding.GetEncoding("iso-8859-1").GetBytes(appKeySecret);
            string auth = Convert.ToBase64String(bytes);
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Authorization",string.Format("{0} {1}","Basic",auth));
            headers.Add("Accept", "application/json");
            var http = MPESAHTTP.GetInstance;
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Add("Authorization", string.Format("{0} {1}", "Basic", auth));
            var response = await client.GetAsync("https://sandbox.safaricom.co.ke/oauth/v1/generate?grant_type=client_credentials");
            var Token = JsonConvert.DeserializeObject<Token>(await response.Content.ReadAsStringAsync());
            Token.ExpiresIn = DateTime.UtcNow.AddMinutes(Token.expires_in % 60); 
           // ts = Token.ExpiresIn - DateTime.UtcNow;//First thread should update the Timespan for all other threads.
            return Token;
        }
        public async void LipaMpesaOnlineStk(LipaMpesaStkPayload lipaMpesaStkPayload)
        {
            var LMO = new
            {
                BusinessShortCode = "174379",
                PartyA = lipaMpesaStkPayload.Msisdn.Remove(0, 1),
                PartyB = "174379",
                TransactionType = "CustomerPayBillOnline",
                Amount = lipaMpesaStkPayload.Amount,
                AccountReference = "ref",
                TransactionDesc = "work hard",
                CallBackURL = "http://9c50ab8b.ngrok.io/api/LipaMpesaOnlineCallback",
                Timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss"),
                Password = MpesaRepository.LipaMpesaOnlinePassKey(),
                PhoneNumber = "254703567954"
            };
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Authorization", string.Format("{0} {1}", "Bearer",await GetToken()));
            headers.Add("Accept", "application/json");
            var client = _httpClientFactory.CreateClient();
            var Token = await GetToken();
            client.DefaultRequestHeaders.Add("Authorization", string.Format("{0} {1}", "Bearer",Token.access_token));
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            var stringContent = new StringContent(JsonConvert.SerializeObject(LMO), Encoding.UTF8,
                                    "application/json");
            var response = await client.PostAsync(lipampesa, stringContent);
            var h = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                var lmoresponse = JsonConvert.DeserializeObject<LMOResponse>(await response.Content.ReadAsStringAsync());
                string sql = "insert into LipaMpesaOnline (Orderid, Merchant_Ref, Mobile_Ref_No) values" +
                    "(@OrdersId, @Merchant_ref, @Mobile_refNo)";

                using (var con = new SqlConnection(new DBCOntext().GetdbConnectionString()))
                {
                    using (var cmd = new SqlCommand(sql,con))
                    {
                        cmd.Parameters.Add("@OrdersId", System.Data.SqlDbType.Int).Value = lipaMpesaStkPayload.Orderid;
                        cmd.Parameters.Add("@Merchant_ref", System.Data.SqlDbType.VarChar,29).Value = lmoresponse.MerchantRequestID;
                        cmd.Parameters.Add("@Mobile_refNo", System.Data.SqlDbType.VarChar, 100).Value = "goals";
                        await con.OpenAsync();
                        cmd.ExecuteNonQuery();
                    }                    
                }
            }
        }
        /**
        public static async Task<Tuple<bool,string,string>>TryPostToMpesa<T>(string url,T payload)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Authorization", string.Format("{0} {1}", "Bearer", await GetToken()));
            headers.Add("Accept", "application/json");
            Tuple<bool,string,string> response;          
            var tobytearray = JsonConvert.SerializeObject(payload[0]);
            response = await MPESAHTTP.GetInstance.SendAsync(url,headers,HttpMethod.Post,tobytearray);
            return response;
        }
    **/


        private string ToJson<T>(T tojson)
        {
            var json = JsonConvert.SerializeObject(tojson);
            return json;
        }
        public static string LipaMpesaOnlinePassKey()
        {
            var password = Convert.ToBase64String(Encoding.UTF8.GetBytes(LipaNaMpesashortcode + passkey + DateTime.UtcNow.ToString("yyyyMMddHHmmss")));
            return password;
        } 
        
    }
    public class Token
    {
        public string access_token { get; set; }
        public double expires_in { get; set; }
        public DateTime ExpiresIn { get; set; } = DateTime.UtcNow;
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
    public class LipaMpesaStkPayload
    {
        public string Msisdn { get; set; }
        public int Orderid { get; set; }
        public decimal Amount { get; set; }
    }

}

