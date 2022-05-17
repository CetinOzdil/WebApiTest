using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebHoster.Interface
{
    public interface IAuthService
    {
        public bool UseCookie { get; set; }
        public string CookieName { get; set; }

        IAuthenticateResponse Authenticate(IAuthenticateRequest authRequest, HttpContext context);
        IEnumerable<IUser> GetAll();
        IUser GetById(int id);
    }
}
