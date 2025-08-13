using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Salon.Application.Tests.DataLoad;
using Salon.Application.Tests.Extensions;
using Salon.Application.Tests.Modules;
using Salon.Application.Tests.Users.Fakes;
using Salon.Application.Users.Interfaces;
using Salon.Domain.Users.Contracts;
using Salon.Domain.Users.Entities;
using Salon.Domain.Users.Repositories;
using Salon.Infra.DbContext;
using System;
using System.Threading.Tasks;

namespace Salon.Application.Tests.Users.Services
{
    [TestFixture]
    public class UserAuthenticationServiceTest
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
        public async Task Should_Authenticate_Valid_User()
        {
            using var scope = _serviceProvider.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<IUserAuthenticationService>();

            var authCommand = new AuthCommand
            {
                Login = UserFake.LOGIN_ROBERT,
                Password = UserFake.PASSWORD_ROBERT
            };

            var result = await service.Authenticate(authCommand);

            result.Error.Should().BeFalse();
            result.Value.Should().NotBeNull();
            result.Value.Should().BeOfType<string>();
        }

        [Test]
        public async Task Should_Return_Error_For_Invalid_Login()
        {
            using var scope = _serviceProvider.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<IUserAuthenticationService>();

            var authCommand = new AuthCommand
            {
                Login = "InvalidLogin",
                Password = UserFake.PASSWORD_ROBERT
            };

            var result = await service.Authenticate(authCommand);

            result.Error.Should().BeTrue();
            result.Value.Should().Be("Username or Password is incorrect!");
        }

        [Test]
        public async Task Should_Return_Error_For_Invalid_Password()
        {
            using var scope = _serviceProvider.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<IUserAuthenticationService>();

            var authCommand = new AuthCommand
            {
                Login = UserFake.LOGIN_ROBERT,
                Password = "InvalidPassword"
            };

            var result = await service.Authenticate(authCommand);

            result.Error.Should().BeTrue();
            result.Value.Should().Be("Username or Password is incorrect!");
        }

        [Test]
        public async Task Should_Return_Error_For_Both_Invalid_Login_And_Password()
        {
            using var scope = _serviceProvider.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<IUserAuthenticationService>();

            var authCommand = new AuthCommand
            {
                Login = "InvalidLogin",
                Password = "InvalidPassword"
            };

            var result = await service.Authenticate(authCommand);

            result.Error.Should().BeTrue();
            result.Value.Should().Be("Username or Password is incorrect!");
        }
    }
}