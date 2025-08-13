using MongoDB.Bson;
using Salon.Domain.Base;
using Salon.Domain.Clients.Contracts;
using System.Threading.Tasks;

namespace Salon.Application.Clients.Interfaces
{
    public interface IClientService
    {
        Task<Result> CreateClient(ClientCommand clientCommand);
        Task<Result> DeleteClient(ObjectId id);
        Task<Result> GetAllClients(int? amount = null);
        Task<Result> GetClientById(ObjectId id);
        Task<Result> UpdateClient(UpdateClientCommand clientCommand);
    }
}