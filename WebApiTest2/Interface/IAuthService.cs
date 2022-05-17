using System;
using System.Collections.Generic;
using System.Text;
using WebApiTest2.Entity;

namespace WebApiTest2.Interface
{
    public interface IAuthService
    {
        public bool UseCookie { get; set; }
        public string CookieName { get; set; }

        AuthenticateResponse Authenticate(AuthenticateRequest authRequest);
        IEnumerable<User> GetAll();
        User GetById(int id);
        
    }
}
