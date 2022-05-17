using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace WebAuth.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        // simple authentication check for endpoints
        // to enable use [Authorize] attribute at endpoints
        // if context has User item added by Auth middleware
        // assumes user is authorized
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.Items["User"];

            if (user == null)
                context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
        }
    }
}