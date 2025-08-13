using FluentAssertions;
using MongoDB.Bson;
using NUnit.Framework;
using Salon.Application.Tests.Users.Fakes;
using Salon.Application.Users.Mappers;
using Salon.Domain.Models.Enums;
using Salon.Domain.Users.Contracts;
using System.Linq;

namespace Salon.Application.Tests.Users.Mappers
{
    [TestFixture]
    public class UserMapperTest
    {
        private UserMapper _mapper;

        [SetUp]
        public void SetUp()
        {
            _mapper = new UserMapper();
        }

        [Test]
        public void Should_Map_UserCommand_To_User_Entity()
        {
            // Arrange
            var command = new UserCommand
            {
                Id = UserFake.IdRobert.ToString(),
                Login = UserFake.LOGIN_ROBERT,
                Password = UserFake.PASSWORD_ROBERT,
                Name = UserFake.NAME_ROBERT,
                Email = UserFake.EMAIL_ROBERT,
                Role = Role.Admin
            };

            // Act
            var result = _mapper.MapCommandToEntity(command);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(UserFake.IdRobert);
            result.Login.Should().Be(UserFake.LOGIN_ROBERT);
            result.Password.Should().Be(UserFake.PASSWORD_ROBERT);
            result.Name.Should().Be(UserFake.NAME_ROBERT);
            result.Email.Should().Be(UserFake.EMAIL_ROBERT);
            result.Role.Should().Be(Role.Admin);
        }

        [Test]
        public void Should_Map_UserCommand_Without_Id_To_User_Entity()
        {
            // Arrange
            var command = new UserCommand
            {
                Login = "NewUser",
                Password = "NewPassword",
                Name = "New User",
                Email = "newuser@example.com",
                Role = Role.User
            };

            // Act
            var result = _mapper.MapCommandToEntity(command);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(ObjectId.Empty);
            result.Login.Should().Be("NewUser");
            result.Password.Should().Be("NewPassword");
            result.Name.Should().Be("New User");
            result.Email.Should().Be("newuser@example.com");
            result.Role.Should().Be(Role.User);
        }

        [Test]
        public void Should_Create_Response_Mapping_Expression()
        {
            // Arrange
            var user = UserFake.GetUserRobert();

            // Act
            var mappingExpression = _mapper.MapResponse();
            var result = new[] { user }.AsQueryable().Select(mappingExpression).First();

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(user.Id.ToString());
            result.Login.Should().Be(user.Login);
            result.Name.Should().Be(user.Name);
            result.Email.Should().Be(user.Email);
            result.Role.Should().Be(user.Role);
        }

        [Test]
        public void Should_Map_Multiple_Users_Using_Response_Expression()
        {
            // Arrange
            var users = new[]
            {
                UserFake.GetUserRobert(),
                UserFake.GetUserTony()
            };

            // Act
            var mappingExpression = _mapper.MapResponse();
            var results = users.AsQueryable().Select(mappingExpression).ToList();

            // Assert
            results.Should().HaveCount(2);
            results[0].Id.Should().Be(UserFake.IdRobert.ToString());
            results[0].Login.Should().Be(UserFake.LOGIN_ROBERT);
            results[1].Id.Should().Be(UserFake.IdTony.ToString());
            results[1].Login.Should().Be(UserFake.LOGIN_TONY);
        }

        [Test]
        public void Should_Handle_Empty_Id_In_Command()
        {
            // Arrange
            var command = new UserCommand
            {
                Id = "",
                Login = "TestUser",
                Password = "TestPassword",
                Name = "Test User",
                Email = "test@example.com",
                Role = Role.User
            };

            // Act
            var result = _mapper.MapCommandToEntity(command);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(ObjectId.Empty);
        }

        [Test]
        public void Should_Handle_Null_Id_In_Command()
        {
            // Arrange
            var command = new UserCommand
            {
                Id = null,
                Login = "TestUser",
                Password = "TestPassword",
                Name = "Test User",
                Email = "test@example.com",
                Role = Role.User
            };

            // Act
            var result = _mapper.MapCommandToEntity(command);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(ObjectId.Empty);
        }
    }
}