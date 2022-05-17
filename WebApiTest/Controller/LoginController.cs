﻿using WebAuth.Attributes;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using WebAuth.Entity;
using WebHoster.Interface;

namespace WebApiTest.Controller
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

        
        [HttpPost("ReqLogin")]
        public IActionResult Login(AuthenticateRequest req)
        {
            var result = _authService.Authenticate(req, HttpContext);

            if (result == null)
                return BadRequest(new { message = "Username or password is not correct!" });

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
