using MongoDB.Bson;
using Salon.Domain.Base;
using Salon.Domain.Users.Contracts;
using System.Threading.Tasks;

namespace Salon.Application.Users.Interfaces
{
    public interface IUserService
    {
        Task<Result> CreateUser(UserCommand userCommand);
        Task<Result> DeleteUser(ObjectId id);
        Task<Result> GetAllUsers();
        Task<Result> GetUserByIdAsync(ObjectId id);
        Task<Result> UpdateUser(UpdateUserCommand UserCommand);
    }
}