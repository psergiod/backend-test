using MongoDB.Bson;
using Salon.Application.Users.Interfaces;
using Salon.Domain.Users.Contracts;
using Salon.Domain.Users.Entities;
using System;
using System.Linq.Expressions;

namespace Salon.Application.Users.Mappers
{
    public class UserMapper : IUserMapper
    {
        public User MapCommandToEntity(UserCommand command)
            => new User(command.Login, command.Password, command.Name)
            { Id = !string.IsNullOrEmpty(command.Id) ? ObjectId.Parse(command.Id) : ObjectId.Empty }
            .InformEmail(command.Email)
            .InformRole(command.Role);

        public Expression<Func<User, UserResponse>> MapResponse()
        {
            return entity => new UserResponse
            {
                Id = entity.Id.ToString(),
                Login = entity.Login,
                Name = entity.Name,
                Email = entity.Email,
                Role = entity.Role
            };
        }

    }
}
