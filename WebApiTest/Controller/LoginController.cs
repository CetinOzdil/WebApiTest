﻿using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using TestApp.Entity;
using WebAuth.Entity;
using WebHoster.Interface.Authentication;

namespace WebApiTest.Controller
{
    [Authorize]
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
        public async Task<IActionResult> Login(AuthenticateRequest req)
        {
            var result = await _authService.Authenticate(req, HttpContext);

            if (result == null)
                return BadRequest(new { message = "Username or password is not correct!" });

            return Ok(result);
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<IEnumerable<IUser>> GetAll(CancellationToken cancellationToken)
        {
            return await Task.Run(() =>
            {
                var result = new List<User>();

                result.Add(new User()
                {
                    Username = "cetin",
                    Password = "123456",
                    FirstName = "Çetin",
                    LastName = "Özdil",
                    Id = 666

                });

                return result;
            }, cancellationToken);
        }
    }
}
