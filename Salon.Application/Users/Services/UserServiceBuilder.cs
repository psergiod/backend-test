using MongoDB.Bson;
using Salon.Application.Clients.Interfaces;
using Salon.Application.Users.Interfaces;
using Salon.Domain.Base;
using Salon.Domain.Clients.Entities;
using Salon.Domain.Users.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Salon.Application.Users.Services
{
    public class UserServiceBuilder : ServiceBuilderBase<User> , IUserServiceBuilder
    {
        public IUserServiceBuilder FilterById(ObjectId id)
        {
            AddFilter(x => x.Id == id);

            return this;
        }

        public IUserServiceBuilder FilterByLogin(string login)
        {
            AddFilter(x=> x.Login == login);

            return this;
        }
    }
}
