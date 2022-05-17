using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using WebApiTest2.Entity;
using WebApiTest2.Interface;

namespace WebApiTest2.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly IAuthService _authService;

        public LoginController(IAuthService authService)
        {
            _authService = authService;
        }

        [AllowAnonymous]
        [HttpPost("ReqLogin")]
        public IActionResult Login(AuthenticateRequest req)
        {
            var result = _authService.Authenticate(req);

            if (result == null)
                return BadRequest(new { message = "Username or password is not correct!" });

            if(_authService.UseCookie)
                HttpContext.Response.Cookies.Append(_authService.CookieName, result.Token);

            return Ok(result);
        }


        [Authorize]
        [HttpGet]
        [Route("GetAll")]
        public IActionResult GetAll(CancellationToken cancellationToken)
        {
            var users = _authService.GetAll();
            return Ok(users);
        }
    }
}
