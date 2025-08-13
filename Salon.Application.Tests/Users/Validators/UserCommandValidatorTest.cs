using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Salon.Application.Tests.DataLoad;
using Salon.Application.Tests.Extensions;
using Salon.Application.Tests.Modules;
using Salon.Application.Tests.Users.Fakes;
using Salon.Application.Users.Validators;
using Salon.Domain.Users.Contracts;
using Salon.Domain.Users.Entities;
using Salon.Domain.Users.Repositories;
using Salon.Infra.DbContext;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Salon.Application.Tests.Users.Validators
{
    [TestFixture]
    public class UserCommandValidatorTest
    {
        private IServiceProvider _serviceProvider;
        private IMongoDbContext _mongoDbContext;

        [OneTimeSetUp]
        public async Task SetUp()
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection = new ModuleLoader(serviceCollection)
                .UseModule<StartupModule>()
                .UseModule<LoadDataModule>()
                .Load();

            _serviceProvider = serviceCollection.BuildServiceProvider();

            await new DataLoadBuilder(_serviceProvider).AddLoadAsync<User>();

            _mongoDbContext = _serviceProvider.GetRequiredService<IMongoDbContext>();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _mongoDbContext.Destroy();
            (_serviceProvider as IDisposable)?.Dispose();
        }

        [Test]
        public async Task Should_Pass_Validation_For_Valid_Command()
        {
            using var scope = _serviceProvider.CreateScope();
            var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
            var validator = new UserCommandValidator(userRepository);

            var command = new UserCommand
            {
                Login = "NewUniqueLogin",
                Password = "ValidPassword123",
                Name = "Test User",
                Email = "test@example.com"
            };

            var result = await validator.ValidateAsync(command);

            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }

        [Test]
        public async Task Should_Fail_Validation_For_Empty_Login()
        {
            using var scope = _serviceProvider.CreateScope();
            var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
            var validator = new UserCommandValidator(userRepository);

            var command = new UserCommand
            {
                Login = "",
                Password = "ValidPassword123",
                Name = "Test User",
                Email = "test@example.com"
            };

            var result = await validator.ValidateAsync(command);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.ErrorMessage == "Login can't be empty!");
        }

        [Test]
        public async Task Should_Fail_Validation_For_Short_Password()
        {
            using var scope = _serviceProvider.CreateScope();
            var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
            var validator = new UserCommandValidator(userRepository);

            var command = new UserCommand
            {
                Login = "UniqueLogin",
                Password = "123", // Short password
                Name = "Test User",
                Email = "test@example.com"
            };

            var result = await validator.ValidateAsync(command);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.ErrorMessage == "Password must be bigger than 5 characters!");
        }

        [Test]
        public async Task Should_Fail_Validation_For_Existing_Login()
        {
            using var scope = _serviceProvider.CreateScope();
            var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
            var validator = new UserCommandValidator(userRepository);

            var command = new UserCommand
            {
                Login = UserFake.LOGIN_ROBERT, // Existing login from test data
                Password = "ValidPassword123",
                Name = "Test User",
                Email = "test@example.com"
            };

            var result = await validator.ValidateAsync(command);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.ErrorMessage == "Login already exist!");
        }

        [Test]
        public async Task Should_Pass_Validation_For_Existing_User_Update_With_Same_Login()
        {
            using var scope = _serviceProvider.CreateScope();
            var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
            var validator = new UserCommandValidator(userRepository);

            var command = new UserCommand
            {
                Id = UserFake.IdRobert.ToString(), // Existing user ID
                Login = UserFake.LOGIN_ROBERT, // Same login as existing user
                Password = "ValidPassword123",
                Name = "Updated Name",
                Email = "updated@example.com"
            };

            var result = await validator.ValidateAsync(command);

            result.IsValid.Should().BeTrue();
        }
    }
}