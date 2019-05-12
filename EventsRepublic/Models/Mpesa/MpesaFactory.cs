
using EventsRepublic.Models.Mpesa;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
namespace EventsRepublic.Models
{
    public class MpesaFactory
    {
        static string app_key = "GE1LxhSyh5xAbOEta7rtpapi6Gl2zhM4";
        static string app_secret = "3F4I1OG7IoxZO3kD";
        static string LipaNaMpesashortcode = "174379";
        static string passkey = "bfb279f9aa9bdbcf158e97dd71a467cd2e0c893059b10f78e6b72ada1ed2c919";
        static string mpesaurl = "https://sandbox.safaricom.co.ke/mpesa/stkpush/v1/processrequest";
        //private static Token Defaulttoken = null;
        private static TimeSpan ts;
       // static readonly SemaphoreSlim fala = new SemaphoreSlim(1, 1); // Gurdian of the threads,only one,
        private static Token MpesaToken = null;
        private static async Task<string> GetToken()
        {
          if (MpesaToken == null)
            {
               //await fala.WaitAsync(); //thread race condition         
               MpesaToken = MpesaToken ?? await GetAyncToken();              
               return MpesaToken.access_token;
            }
            ts = MpesaToken.ExpiresIn - DateTime.UtcNow;
            if (0 < ts.Minutes) //token expires in 3599 sec,renew after 57 minutes,rather than 60 min to avoid latency and other thread issues
            {
                return MpesaToken.access_token;
            }                           
            MpesaToken = (0 < ts.Minutes) ? MpesaToken : await GetAyncToken();        
            return MpesaToken.access_token;
        }
        private async static Task<Token> GetAyncToken()
        {
            string appKeySecret = app_key + ":" + app_secret;
            byte[] bytes = Encoding.GetEncoding("iso-8859-1").GetBytes(appKeySecret);
            string auth = Convert.ToBase64String(bytes);
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Authorization",string.Format("{0} {1}","Basic",auth));
            headers.Add("Accept", "application/json");
            var http = MPESAHTTP.GetInstance;
            var response = await http.SendAsync("https://sandbox.safaricom.co.ke/oauth/v1/generate?grant_type=client_credentials",headers,new HttpMethod("GET"));
            var Token = JsonConvert.DeserializeObject<Token>(response.Item3);
            Token.ExpiresIn = DateTime.UtcNow.AddMinutes(Token.expires_in % 60); 
            ts = Token.ExpiresIn - DateTime.UtcNow;//First thread should update the Timespan for all other threads.
            return Token;
        }       
        public async Task<Tuple<bool,string,string>> TryPostToMpesa<T>(string url,params T []  payload)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Authorization", string.Format("{0} {1}", "Bearer", await GetToken()));
            headers.Add("Accept", "application/json");
             Tuple<bool,string,string> response;
            //headers.Add("Accept", "application/json");
            // headers.Add("Content-Type","application/json");
          
                var tobytearray = JsonConvert.SerializeObject(payload[0]);
                response = await MPESAHTTP.GetInstance.SendAsync(url,headers,HttpMethod.Post,tobytearray);
                return response;
        }

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
        ByteArrayContent ToByteArray(string tobytearraycontent)
        {
            var json = JsonConvert.SerializeObject(tobytearraycontent);
            var bytearrayMessage = new ByteArrayContent(Encoding.UTF8.GetBytes(json));
            return bytearrayMessage;
        }     
    }
    public class Token
    {
        public string access_token { get; set; }
        public double expires_in { get; set; }
        public DateTime ExpiresIn { get; set; }
    }
}

