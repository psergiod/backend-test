using Salon.Domain.Models.Enums;
using System;
using System.Collections.Generic;

namespace Salon.Domain.Clients.Contracts
{
    public class ClientResponse
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<string> ContactNumbers { get; set; } = new List<string>();
        public DateTime BirthDate { get; set; }
        public Gender Gender { get; set; }
        public string Email { get; set; }
    }
}
