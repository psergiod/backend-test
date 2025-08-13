using MongoDB.Bson;
using Salon.Domain.Models.Enums;
using Salon.Domain.Users.Contracts;
using System;
using BCryptNet = BCrypt.Net.BCrypt;
using UserEntity = Salon.Domain.Users.Entities.User;

namespace Salon.Application.Tests.Users.Fakes
{
    public static class UserFake
    {
        public static ObjectId IdFoo = ObjectId.Parse("63e5cdeebf4932a2d1e8ba81");
        public const string LOGIN_FOO = "Foo";
        public const string PASSWORD_FOO = "PASSWORD_Bar";
        public const string NAME_FOO = "Foo_Name";
        public const string EMAIL_FOO = "Foo@gmail.com";

        public static ObjectId IdJames = ObjectId.Parse("649205a22524c80b79e0d19d");
        public const string LOGIN_JAMES = "James";
        public const string PASSWORD_JAMES = "PASSWORD_James";
        public const string NAME_JAMES = "James Jhonson";
        public const string NEW_NAME_JAMES = "James Jhonson Junior";
        public const string EMAIL_JAMES = "james.jhonsons@hotmail.com";

        public static ObjectId IdRobert = ObjectId.Parse("649205a22524c80b79e0d1a1");
        public const string LOGIN_ROBERT = "Robert";
        public const string PASSWORD_ROBERT = "PASSWORD_ROBERT";
        public const string NAME_ROBERT = "Robert Jhonson";
        public const string EMAIL_ROBERT = "Robert.Smith@outlook.com";

        public static ObjectId IdTony = ObjectId.Parse("649205a22524c80b79e0d1ca");
        public const string LOGIN_TONY = "Tony";
        public const string PASSWORD_TONY = "PASSWORD_Tony";
        public const string NAME_TONY = "Tony Stark";
        public const string NEW_NAME_TONY = "Anthony Stark";
        public const string EMAIL_TONY = "Tony.Stark@hotmail.com";

        public static UserEntity GetUserEntity(string login, string password, string name, string email, ObjectId? id = null)
        {
            var user = new UserEntity(login, password, name)
                .InformEmail(email)
                .InformRole(Role.Admin);
            user.Id = id ?? ObjectId.Empty;

            return user;
        }

        public static UserEntity GetUserEntity()
            => GetUserEntity(LOGIN_FOO, BCryptNet.HashPassword(PASSWORD_FOO), NAME_FOO, EMAIL_FOO, IdFoo);

        public static UserEntity GetUserRobert()
            => GetUserEntity(LOGIN_ROBERT, BCryptNet.HashPassword(PASSWORD_ROBERT), NAME_ROBERT, EMAIL_ROBERT, IdRobert);

        public static UserCommand GetUserCommand()
        {
            return new UserCommand()
            {
                Login = LOGIN_JAMES,
                Password = PASSWORD_JAMES,
                Name = NAME_JAMES,
                Email = EMAIL_JAMES
            };
        }

        public static UpdateUserCommand GetUpdateUserCommandTony()
        {
            return new UpdateUserCommand()
            {
                Login = LOGIN_TONY,
                Password = PASSWORD_TONY,
                Name = NEW_NAME_TONY,
                Email = EMAIL_TONY,
                Id = IdTony.ToString()
            };
        }

        public static UserResponse GetUserResponseRobert()
            => new UserResponse()
            {
                Login = LOGIN_ROBERT,
                Email = EMAIL_ROBERT,
                Id = IdRobert.ToString(),
                Name = NAME_ROBERT,
                Role = Role.Admin
            };

        public static UserEntity GetUserJames()
            => GetUserEntity(LOGIN_JAMES, BCryptNet.HashPassword(PASSWORD_JAMES), NAME_JAMES, EMAIL_JAMES, IdJames);

        public static UserEntity GetUserTony()
            => GetUserEntity(LOGIN_TONY, BCryptNet.HashPassword(PASSWORD_TONY), NAME_TONY, EMAIL_TONY, IdTony);

        public static UserEntity GetUserTonyUpdated()
    => GetUserEntity(LOGIN_TONY, BCryptNet.HashPassword(PASSWORD_TONY), NEW_NAME_TONY, EMAIL_TONY, IdTony);
    }
}
