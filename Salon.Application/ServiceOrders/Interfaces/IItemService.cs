using MongoDB.Bson;
using Salon.Domain.Base;
using Salon.Domain.ServiceOrders.Contracts;
using System.Threading.Tasks;

namespace Salon.Application.ServiceOrders.Interfaces
{
    public interface IItemService
    {
        Task<Result> CreateItem(ItemCommand item);
        Task<Result> GetAllAsync();
        Task<Result> GetItemById(ObjectId id);
        Task<Result> RemoveItem(ObjectId id);
    }
}