using EventsRepublic.Models;
using EventsRepublic.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using static EventsRepublic.Middleware.Authentication;

namespace EventsRepublic.Attributes
{
    public class CustomAuthorizeFilter : IAsyncAuthorizationFilter
    {
        enum UserRolesenum
        {
            Admin,
            Organiser,
            Guest
        }
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            if (context.Filters.Any(item => item is IAllowAnonymousFilter))
            {
                return;
            }
            object User;
            //bool userhasrole = false;
            if (context.HttpContext.Items.TryGetValue("principaluser", out User))
            {
                JsonWebToken jwt = new JsonWebToken();
                var userroles = await jwt.GetRoles((Payload)User);
                var user = (Payload)User;
                if (!(Icontains.ContainsRole(userroles)))
                {
                    context.Result = new UnauthorizedResult();
                }
            }
            else
            {
                context.Result = new UnauthorizedResult();
            }
           
        }
       


    }
    public static class Ienumcheck
    {
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable)
        {
            return enumerable == null || !enumerable.Any();
        }
    }

    public static class Icontains
    {
        public static bool ContainsRole(this IEnumerable<string> enumerable)
        {
            if (enumerable == null || !enumerable.Any())
            {
                return false;
            }
            bool inrole = enumerable.Any(arg => arg == "Admin" || arg == "Organiser");
            return inrole;
        }
    }
}
