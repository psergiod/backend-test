using MongoDB.Bson;
using Salon.Domain.Base;
using Salon.Domain.Users.Entities;

namespace Salon.Application.Users.Interfaces
{
    public interface IUserServiceBuilder : IServiceBuilderBase<User>
    {
        IUserServiceBuilder FilterById(ObjectId id);
        IUserServiceBuilder FilterByLogin(string login);

    }
}
