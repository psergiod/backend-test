using Salon.Domain.Users.Contracts;
using Salon.Domain.Users.Entities;
using System;
using System.Linq.Expressions;

namespace Salon.Application.Users.Interfaces
{
    public interface IUserMapper
    {
        User MapCommandToEntity(UserCommand command);
        Expression<Func<User, UserResponse>> MapResponse();
    }
}