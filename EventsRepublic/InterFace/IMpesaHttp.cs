using System.Net.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventsRepublic.InterFace
{
    public interface IMpesaHttp
    {
        Task<Tuple<bool, string, string>> SendAsync(string url, Dictionary<string, string> headers, HttpMethod httpmethod, params string[] value);
    }
}
