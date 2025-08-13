using MongoDB.Bson;
using MongoDB.Driver;
using Salon.Domain.Users.Entities;
using Salon.Domain.Users.Repositories;
using Salon.Infra.CollectionDefinitions;
using Salon.Infra.DbContext;
using System.Threading.Tasks;

namespace Salon.Infra.Repositories
{
    public class UserRepository : Repository<User>, IUserRemovedRepository
    {
        public UserRepository(
            IMongoDbContext dbContext,
            bool showRemoved = false)
        : base(dbContext, showRemoved)
        { }

        public async Task<User> GetUserByLoginAsync(string login)
        {
            var filter = Builders<User>.Filter.Eq(x => x.Login, login);

            return await GetFirstOrDefaultByFilterAsync(filter);
        }

        public async Task<User> GetUserByLoginAsync(string login, ObjectId id)
        {
            var filter = Builders<User>.Filter.Eq(x => x.Login, login);
            filter &= Builders<User>.Filter.Ne(x => x.Id, id);

            return await GetFirstOrDefaultByFilterAsync(filter);
        }
    }
}
