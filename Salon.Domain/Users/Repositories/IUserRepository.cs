using MongoDB.Bson;
using Salon.Domain.Base;
using Salon.Domain.Users.Entities;
using System.Threading.Tasks;

namespace Salon.Domain.Users.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetUserByLoginAsync(string login);
        Task<User> GetUserByLoginAsync(string login, ObjectId id);
    }
}
