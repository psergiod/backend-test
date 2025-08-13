using MongoDB.Bson;
using Salon.Domain.Base;
using Salon.Domain.Clients.Entities;
using System.Threading.Tasks;

namespace Salon.Domain.Clients.Repositories
{
    public interface IClientRepository : IRepository<Client>
    {
        Task<Client> GetClientByEmailAndIdAsync(string email, ObjectId id);
        Task<Client> GetClientByEmailAsync(string email);
    }
}
