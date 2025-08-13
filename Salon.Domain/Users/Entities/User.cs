using MongoDB.Bson;
using Salon.Domain.Models;
using Salon.Domain.Models.Enums;
using System;

namespace Salon.Domain.Users.Entities
{
    public class User : Entity<ObjectId>
    {
        public string Name { get; internal set; }
        public string Email { get; internal set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public Role Role { get; set; }
        public User(string login, string password, string name)
        {
            Name = name;
            Login = login;
            Password = password;
        }

        public User InformEmail(string email)
        {
            Email = email;
            return this;
        }

        public User InformRole(Role role)
        {
            Role = role;
            return this;
        }
    }
}
