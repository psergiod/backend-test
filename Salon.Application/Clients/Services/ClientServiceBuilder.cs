using MongoDB.Bson;
using Salon.Application.Clients.Interfaces;
using Salon.Domain.Base;
using Salon.Domain.Clients.Entities;

namespace Salon.Application.Clients.Services
{
    public class ClientServiceBuilder : ServiceBuilderBase<Client>, IClientServiceBuilder
    {
        public IClientServiceBuilder FilterById(ObjectId id)
        {
            AddFilter(x => x.Id == id);

            return this;
        }
    }
}
