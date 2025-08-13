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
using System.Threading.Tasks;

namespace Salon.Application.Tests.Users.Validators
{
    [TestFixture]
    public class UpdateUserCommandValidatorTest
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
        public async Task Should_Pass_Validation_For_Valid_Update_Command()
        {
            using var scope = _serviceProvider.CreateScope();
            var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
            var validator = new UpdateUserCommandValidator(userRepository);

            var command = new UpdateUserCommand
            {
                Id = UserFake.IdRobert.ToString(),
                Name = "Updated Name",
                Email = "updated@example.com"
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
            var validator = new UpdateUserCommandValidator(userRepository);

            var command = new UpdateUserCommand
            {
                Login = "",
                Name = "Updated Name",
                Email = "updated@example.com"
            };

            var result = await validator.ValidateAsync(command);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.ErrorMessage == "Login can't be empty!");
        }

        [Test]
        public async Task Should_Allow_Same_User_To_Keep_Their_Login()
        {
            using var scope = _serviceProvider.CreateScope();
            var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
            var validator = new UpdateUserCommandValidator(userRepository);

            var command = new UpdateUserCommand
            {
                Id = UserFake.IdRobert.ToString(),
                Login = UserFake.LOGIN_ROBERT, // Same user keeping their login
                Name = "Updated Name",
                Email = "updated@example.com"
            };

            var result = await validator.ValidateAsync(command);

            result.IsValid.Should().BeTrue();
        }

        [Test]
        public async Task Should_Fail_Validation_For_Login_Used_By_Another_User()
        {
            using var scope = _serviceProvider.CreateScope();
            var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
            var validator = new UpdateUserCommandValidator(userRepository);

            var command = new UpdateUserCommand
            {
                Id = UserFake.IdRobert.ToString(),
                Login = UserFake.LOGIN_TONY, // Different user's login
                Name = "Updated Name",
                Email = "updated@example.com"
            };

            var result = await validator.ValidateAsync(command);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.ErrorMessage == "Login already exist!");
        }
    }
}