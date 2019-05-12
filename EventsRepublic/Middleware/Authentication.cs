using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using EventsRepublic.Models;
using EventsRepublic.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;

namespace EventsRepublic.Middleware
{
    //WatchMan 
    public class Authentication
    {
        public class AuthDenied
        {
            public int StatusCode = 401;
            public string Message = "You Shall Not Pass";
        }
        private readonly RequestDelegate _next;

        public Authentication(RequestDelegate next)
        {
            _next = next;
        }
        //
        public async Task Invoke(HttpContext httpContext)
        {
           
                var authHeader = (string)httpContext.Request.Headers["Authorization"];
            if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                {
                if (authHeader.Substring("Bearer ".Length).Trim() == "null")
                {
                    httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    // httpContext.Response.Headers.Add("TokenExpired","");
                    httpContext.Response.ContentType = "application/json";
                    string json = JsonConvert.SerializeObject(new { ReasonCode = "invalid token" });
                    await httpContext.Response.WriteAsync(json);
                    return;
                }
                    //get token
                    string token = authHeader.Split(' ')[1];
                    var tokenTuple = JsonWebToken.ValidateToken(token, true);
                    if (!tokenTuple.Item1)
                    {
                        httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                       // httpContext.Response.Headers.Add("TokenExpired","");
                        httpContext.Response.ContentType = "application/json";
                        string json = JsonConvert.SerializeObject(new { ReasonCode = tokenTuple.Item2});
                        await httpContext.Response.WriteAsync(json);
                        return;
                    }
                //if true
                    httpContext.Items.Add("principaluser", JsonConvert.DeserializeObject<Payload>(tokenTuple.Item2));

                   // httpContext.Items.Add("userroles", userroles);                  
            }
            if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Basic ", StringComparison.OrdinalIgnoreCase))
            {
                string encodedUsernamePassword = authHeader.Substring("Basic ".Length).Trim();
                Encoding encoding = Encoding.GetEncoding("iso-8859-1");
                string usernamePassword = encoding.GetString(Convert.FromBase64String(encodedUsernamePassword));

                int seperatorIndex = usernamePassword.IndexOf(':');

                //var username = usernamePassword.Substring(0, seperatorIndex);
               // var password = usernamePassword.Substring(seperatorIndex + 1);
                httpContext.Items.Add("usertoregister", seperatorIndex);
            }

                await _next(httpContext);                        
        }
        
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class AuthenticationExtensions
    {
        public static IApplicationBuilder UseAuthentication(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<Authentication>();
        }
    }
   

}
