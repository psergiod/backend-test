using MongoDB.Bson;
using MongoDB.Driver;
using Salon.Domain.Clients.Entities;
using Salon.Domain.Clients.Repositories;
using Salon.Infra.DbContext;
using System.Threading.Tasks;

namespace Salon.Infra.Repositories
{
    public class ClientRepository : Repository<Client>, IClientRemovedRepository
    {
        public ClientRepository(
            IMongoDbContext dbContext,
            bool showRemoved = false)
            : base(dbContext, showRemoved)
        { }

        public async Task<Client> GetClientByEmailAsync(string email)
        {
            var filter = Builders<Client>.Filter.Eq(x => x.Email, email);

            return await GetFirstOrDefaultByFilterAsync(filter);
        }

        public async Task<Client> GetClientByEmailAndIdAsync(string email, ObjectId id)
        {
            var filter = Builders<Client>.Filter.Eq(x => x.Email, email);
            filter &= Builders<Client>.Filter.Ne(x => x.Id, id);

            return await GetFirstOrDefaultByFilterAsync(filter);
        }


    }
}
