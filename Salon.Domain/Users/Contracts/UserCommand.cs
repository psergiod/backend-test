using Salon.Domain.Base;
using Salon.Domain.Models.Enums;
using System;
using System.Collections.Generic;

namespace Salon.Domain.Users.Contracts
{
    public class UserCommand : CommandBase
    {
        public string Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public Role Role { get; set; }
        public string Email { get; set; }

        public UserCommand SetOperation(Operation operation)
        {
            Operation = operation;
            return this;
        }
    }
}
