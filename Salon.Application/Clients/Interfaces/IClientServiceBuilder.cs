using MongoDB.Bson;
using Salon.Domain.Base;
using Salon.Domain.Clients.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Salon.Application.Clients.Interfaces
{
    public interface IClientServiceBuilder : IServiceBuilderBase<Client>
    {
        IClientServiceBuilder FilterById(ObjectId id);
    }
}
