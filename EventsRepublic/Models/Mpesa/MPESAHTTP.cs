using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static EventsRepublic.Models.Mpesa.MPESA;

namespace EventsRepublic.Models.Mpesa
{
    /**
     * create a singleton class hhtp client
     * make method generic to accomodate all http calls from diferent methods
     * Rem to use singleton Dependency injection later
    **/
    public  class MPESAHTTP
    {
        private readonly static HttpClient httpClient = new HttpClient();
        public static MPESAHTTP GetInstance { get; } = new MPESAHTTP();
        private MPESAHTTP() {  }

        public async Task<Tuple<bool,string,string>> SendAsync(string url,Dictionary<string,string>headers,HttpMethod httpmethod,params string[]value)
        {
            try
            {
                using (var reqMessage = new HttpRequestMessage(httpmethod, url))
                {
                    if (!(value == null || value.Length == 0))
                    {
                        //var k = new ByteArrayContent(Encoding.UTF8.GetBytes(value[0]));
                        // reqMessage.Content = k;
                        reqMessage.Content = new StringContent(value[0]);
                        reqMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                        // reqMessage.Content.Headers.Add("Content-Type","application/json");

                    }
                    foreach (KeyValuePair<string, string> pair in headers)
                    {
                        reqMessage.Headers.Add(pair.Key, pair.Value);
                    }
                    var response = await httpClient.SendAsync(reqMessage);
                    var results = await response.Content.ReadAsStringAsync();
                    var responsestatus = response.IsSuccessStatusCode;
                    //
                   return new Tuple<bool,string,string>(responsestatus,response.ReasonPhrase,results);                 
                }
            }
            catch (Exception ex)
            {

                throw;
            }
           
        }
       

    }
}

