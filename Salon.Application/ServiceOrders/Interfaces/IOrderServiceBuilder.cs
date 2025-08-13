using MongoDB.Bson;
using Salon.Application.ServiceOrders.Services;
using Salon.Domain.Base;
using Salon.Domain.ServiceOrders.Entities;

namespace Salon.Application.ServiceOrders.Interfaces
{
    public interface IOrderServiceBuilder : IServiceBuilderBase<ServiceOrder>
    {
        OrderServiceBuilder FilterById(ObjectId id);
        OrderServiceBuilder FilterByClientId(ObjectId clientId);
    }
}