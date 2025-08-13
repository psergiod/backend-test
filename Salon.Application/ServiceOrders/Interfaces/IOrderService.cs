using MongoDB.Bson;
using Salon.Domain.Base;
using Salon.Domain.ServiceOrders.Contracts;
using System.Threading.Tasks;

namespace Salon.Application.ServiceOrders.Interfaces
{
    public interface IOrderService
    {
        Task<Result> CreateOrder(ServiceOrderCommand command);
        Task<Result> DeleteOrder(ObjectId id);
        Task<Result> GetAllOrders();
        Task<Result> GetOrderByIdAsync(ObjectId id);
        Task<Result> GetOrdersByClientId(ObjectId clientId);
    }
}