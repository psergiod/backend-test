using Salon.Domain.Models;
using Salon.Domain.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Salon.Domain.Clients.Entities
{
    public class Client : Person
    {
        public List<string> ContactNumbers { get; internal set; } = new List<string>();
        public Client(string name)
            : base(name) { }

        public Client InformBirthDate(DateTime birthDate)
        {
            BirthDate = birthDate.Date;
            return this;
        }

        public Client InformGender(Gender gender)
        {
            Gender = gender;
            return this;
        }

        public Client InformContactNumber(string contactNumber)
        {
            if (!ContactNumbers.Any())
                ContactNumbers = new List<string>();

            ContactNumbers.Add(contactNumber);
            return this;
        }

        public Client InformContactNumbers(List<string> contactNumbers)
        {
            contactNumbers.ForEach(x => InformContactNumber(x));
            return this;
        }

        public Client InformEmail(string email)
        {
            Email = email;
            return this;
        }
    }
}
