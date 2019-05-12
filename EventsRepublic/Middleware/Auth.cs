using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace EventsRepublic.Middleware
{
    // I may need to install the Microsoft.AspNetCore.Http.Abstractions 
    //Use this middleware like a router
    public class Auth 
    {
        private readonly RequestDelegate _next;

        public Auth(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext httpContext)
        {
            //
            var url = httpContext.Request.Path.Value.ToString();
            //if user the uri is for the general..Let them Pass
            return _next(httpContext);
        }
    }

    // method used to add the middleware to the HTTP request pipeline.
    public static class AuthExtensions
    {
        public static IApplicationBuilder UseAuth(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<Auth>();
        }
    }
}
