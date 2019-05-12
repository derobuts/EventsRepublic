using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventsRepublic.Models
{
    public class RolesAttribute : AuthorizeAttribute , IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
          
        }
        
        private bool Authorize(HttpContext actioncontext)
        {
            try
            {
                var encodedstring = actioncontext.Request.Headers.TryGetValue("Derob", out var headerValues);
                if (encodedstring)
                {
                    var header = headerValues.First();
                }
            }
            catch (Exception)
            {

                throw;
            }
            return true;
        }

    }
}
