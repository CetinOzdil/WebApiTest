﻿using System.Text.Json.Serialization;
using WebHoster.Interface.Authentication;

namespace TestApp.Entity
{
    public class User : IUser
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }

        [JsonIgnore]
        public string Password { get; set; }
    }
}
