using MongoDB.Bson;
using Salon.Application.ServiceOrders.Interfaces;
using Salon.Domain.Base;
using Salon.Domain.ServiceOrders.Entities;

namespace Salon.Application.ServiceOrders.Services
{
    public class OrderServiceBuilder : ServiceBuilderBase<ServiceOrder>, IOrderServiceBuilder
    {
        public OrderServiceBuilder FilterById(ObjectId id)
        {
            AddFilter(x => x.Id == id);

            return this;
        }

        public OrderServiceBuilder FilterByClientId(ObjectId clientId)
        {
            AddFilter(x=> x.ClientId == clientId); 
            
            return this;
        }
    }
}
