using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Salon.Application.Clients.Interfaces;
using Salon.Application.Tests.Clients.Fakes;
using Salon.Application.Tests.DataLoad;
using Salon.Application.Tests.Extensions;
using Salon.Application.Tests.Modules;
using Salon.Domain.Clients.Contracts;
using Salon.Domain.Clients.Entities;
using Salon.Domain.Clients.Repositories;
using Salon.Infra.DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Salon.Application.Tests.Clients.Services
{
    public class ClientServiceTest
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

            await new DataLoadBuilder(_serviceProvider).AddLoadAsync<Client>();

            _mongoDbContext = _serviceProvider.GetRequiredService<IMongoDbContext>();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _mongoDbContext.Destroy();
        }

        [Test]
        public async Task Should_Create_Client_From_Command()
        {
            var command = ClientFakes.GetClientCommand();

            using var scope = _serviceProvider.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<IClientService>();
            var repository = scope.ServiceProvider.GetRequiredService<IClientRepository>();
            await service.CreateClient(command);

            var created = await repository.GetClientByEmailAsync(command.Email);
            var expected = ClientFakes.GetClientJames();

            created.Should().HaveEquivalentMembers(expected,
                o => o
                .Excluding(x => x.Id));
        }

        [Test]
        public async Task Should_Flag_Client_As_Removed()
        {
            using var scope = _serviceProvider.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<IClientService>();
            var repository = scope.ServiceProvider.GetRequiredService<IClientRemovedRepository>();

            var client = await repository.GetClientByEmailAsync(ClientFakes.EMAIL_MIKE);

            client.Removed.Should().BeFalse();

            await service.DeleteClient(client.Id);

            var removedUser = await repository.GetClientByEmailAsync(ClientFakes.EMAIL_MIKE);

            removedUser.Removed.Should().BeTrue();
        }

        [Test]
        public async Task Should_Get_All_Clients()
        {
            using var scope = _serviceProvider.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<IClientService>();
            var repository = scope.ServiceProvider.GetRequiredService<IClientRepository>();

            var allFromRepository = (await repository.GetAllAsync()).Count();

            var result = await service.GetAllClients();

            var client = (List<ClientResponse>)result.Value;
            client.Count().Should().Be(allFromRepository);
        }

        [Test]
        public async Task Should_Get_ClientResponse_By_Id()
        {
            using var scope = _serviceProvider.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<IClientService>();
            var repository = scope.ServiceProvider.GetRequiredService<IClientRepository>();

            var result = await service.GetClientById(ClientFakes.IdBob);

            var response = result.Value as ClientResponse;
            var expected = ClientFakes.GetClientResponseBob();

            response.Should().HaveEquivalentMembers(expected);
        }

        [Test]
        public async Task Should_Update_Client_From_Command()
        {
            var command = ClientFakes.GetTonyUpdateCommand();

            using var scope = _serviceProvider.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<IClientService>();
            var repository = scope.ServiceProvider.GetRequiredService<IClientRepository>();
            await service.UpdateClient(command);

            var updated = await repository.GetByIdAsync(new MongoDB.Bson.ObjectId(command.Id));
            var expected = ClientFakes.GetClientTonyUpdated();

            updated.Should().HaveEquivalentMembers(expected);
        }

        [Test]
        public async Task Should_Validate_Invalid_Email()
        {
            using var scope = _serviceProvider.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<IClientService>();
            var repository = scope.ServiceProvider.GetRequiredService<IClientRepository>();

            var command = ClientFakes.GetClientCommand();
            command.Email = "invalid.email@invalid@";

            var result = await service.CreateClient(command);

            result.Error.Should().BeTrue();
            (result.Value as List<string>).Single().Should().Be($"{command.Email} is not a valid Email!");
        }

        [Test]
        public async Task Should_Validate_Email_In_Use()
        {
            using var scope = _serviceProvider.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<IClientService>();
            var repository = scope.ServiceProvider.GetRequiredService<IClientRepository>();

            var command = ClientFakes.GetClientCommand();
            command.Email = ClientFakes.EMAIL_BOB;

            var result = await service.CreateClient(command);

            result.Error.Should().BeTrue();
            (result.Value as List<string>).Single().Should().Be($"Email {command.Email} Already in use!");
        }

        [Test]
        public async Task Should_Validate_Invalid_Id_When_Updating()
        {
            using var scope = _serviceProvider.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<IClientService>();
            var repository = scope.ServiceProvider.GetRequiredService<IClientRepository>();

            var command = ClientFakes.GetBobUpdateCommand("invalidId");

            var result = await service.UpdateClient(command);

            result.Error.Should().BeTrue();
            (result.Value as List<string>).Single().Should().Be($"Id is invalid!");
        }
    }
}
