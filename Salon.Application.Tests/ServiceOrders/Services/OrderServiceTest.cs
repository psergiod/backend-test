using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using NUnit.Framework;
using Salon.Application.ServiceOrders.Interfaces;
using Salon.Application.Tests.DataLoad;
using Salon.Application.Tests.Extensions;
using Salon.Application.Tests.Modules;
using Salon.Application.Tests.ServiceOrders.Fakes;
using Salon.Domain.ServiceOrders.Entities;
using Salon.Domain.ServiceOrders.Repositories;
using Salon.Infra.DbContext;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Salon.Application.Tests.ServiceOrders.Services
{
    [TestFixture]
    public class OrderServiceTest
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

            await new DataLoadBuilder(_serviceProvider).AddLoadAsync<ServiceOrder>();

            _mongoDbContext = _serviceProvider.GetRequiredService<IMongoDbContext>();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _mongoDbContext.Destroy();
            (_serviceProvider as IDisposable)?.Dispose();
        }

        [Test]
        public async Task Should_Create_Order_From_Valid_Command()
        {
            using var scope = _serviceProvider.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<IOrderService>();
            var repository = scope.ServiceProvider.GetRequiredService<IServiceOrderRepository>();

            var command = ServiceOrderFake.GetServiceOrderCommand();

            var result = await service.CreateOrder(command);

            result.Error.Should().BeFalse();
            result.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        [Test]
        public async Task Should_Return_Error_For_Invalid_Command()
        {
            using var scope = _serviceProvider.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<IOrderService>();

            var command = ServiceOrderFake.GetInvalidServiceOrderCommand();

            var result = await service.CreateOrder(command);

            result.Error.Should().BeTrue();
        }

        [Test]
        public async Task Should_Get_All_Orders()
        {
            using var scope = _serviceProvider.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<IOrderService>();

            var result = await service.GetAllOrders();

            result.Error.Should().BeFalse();
            result.StatusCode.Should().Be(HttpStatusCode.OK);
            result.Value.Should().NotBeNull();
        }

        [Test]
        public async Task Should_Get_Order_By_Id()
        {
            using var scope = _serviceProvider.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<IOrderService>();
            var repository = scope.ServiceProvider.GetRequiredService<IServiceOrderRepository>();

            // Create an order first
            var orderEntity = ServiceOrderFake.GetServiceOrderEntity();
            await repository.InsertAsync(orderEntity);

            var result = await service.GetOrderByIdAsync(orderEntity.Id);

            result.Error.Should().BeFalse();
            result.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        public async Task Should_Delete_Order_By_Id()
        {
            using var scope = _serviceProvider.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<IOrderService>();
            var repository = scope.ServiceProvider.GetRequiredService<IServiceOrderRepository>();

            // Create an order first
            var orderEntity = ServiceOrderFake.GetServiceOrderEntity();
            await repository.InsertAsync(orderEntity);

            var result = await service.DeleteOrder(orderEntity.Id);

            result.Error.Should().BeFalse();
            result.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        public async Task Should_Return_Error_When_Deleting_Invalid_Order_Id()
        {
            using var scope = _serviceProvider.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<IOrderService>();

            var invalidId = ObjectId.GenerateNewId();

            var result = await service.DeleteOrder(invalidId);

            result.Error.Should().BeTrue();
            result.Value.Should().Be("Order Invalid");
        }

        [Test]
        public async Task Should_Get_Orders_By_Client_Id()
        {
            using var scope = _serviceProvider.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<IOrderService>();
            var repository = scope.ServiceProvider.GetRequiredService<IServiceOrderRepository>();

            // Create orders for different clients
            var order1 = ServiceOrderFake.GetServiceOrderEntity();
            var order2 = ServiceOrderFake.GetServiceOrderForClient2();
            
            await repository.InsertAsync(order1);
            await repository.InsertAsync(order2);

            var result = await service.GetOrdersByClientId(ServiceOrderFake.ClientId1);

            result.Error.Should().BeFalse();
            result.Value.Should().NotBeNull();
        }
    }
}