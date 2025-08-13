using MongoDB.Bson;
using Salon.Domain.Models.Enums;
using System;
using System.Collections.Generic;

namespace Salon.Domain.Models
{
    public abstract class Person : Entity<ObjectId>
    {
        public string Name { get; internal set; }
        public DateTime BirthDate { get; internal set; }
        public Gender Gender { get; internal set; }
        public string Email { get; internal set; }

        protected Person(string name)
        {
            Name = name;
        }
    }
}
