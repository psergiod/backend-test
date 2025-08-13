using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver.Linq;
using NUnit.Framework;
using Salon.Application.ServiceOrders.Interfaces;
using Salon.Application.Tests.DataLoad;
using Salon.Application.Tests.Extensions;
using Salon.Application.Tests.Modules;
using Salon.Application.Tests.ServiceOrders.Fakes;
using Salon.Domain.Base;
using Salon.Domain.ServiceOrders.Contracts;
using Salon.Domain.ServiceOrders.Entities;
using Salon.Infra.DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Salon.Application.Tests.ServiceOrders.Services
{
    [TestFixture]
    public class ItemServiceTest
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

            await new DataLoadBuilder(_serviceProvider).AddLoadAsync<Item>();

            _mongoDbContext = _serviceProvider.GetRequiredService<IMongoDbContext>();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _mongoDbContext.Destroy();
        }

        [Test]
        public async Task Should_Create_Item()
        {
            var command = ItemFake.GetItemCommandPedicure();

            using var scope = _serviceProvider.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<IItemService>();
            var repository = scope.ServiceProvider.GetRequiredService<IRepository<Item>>();

            await service.CreateItem(command);

            var created = (await repository.GetAllAsync()).Where(x => x.Description == command.Description).First();

            var expected = ItemFake.GetItemPedicure(created.Id);

            created.Should().HaveEquivalentMembers(expected);
        }

        [Test]
        public async Task Should_Get_ItemResponse_By_Id()
        {
            using var scope = _serviceProvider.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<IItemService>();
            var repository = scope.ServiceProvider.GetRequiredService<IRepository<Item>>();

            var id = ItemFake.GetItemBrazilianBlowout().Id;

            var result = await service.GetItemById(id);
            var expected = ItemFake.GetItemResponseBrazilianBlowout();

            var actual = (ItemResponse)result.Value;
            actual.Should().HaveEquivalentMembers(expected);
        }

        [Test]
        public async Task Should_Get_All_Items()
        {
            using var scope = _serviceProvider.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<IItemService>();
            var repository = scope.ServiceProvider.GetRequiredService<IRepository<Item>>();

            var allFromRepository = await repository.GetAllAsync();

            var result = await service.GetAllAsync();

            var items = (List<ItemResponse>)result.Value;
            items.Count.Should().Be(allFromRepository.Count());
        }

        [Test]
        public async Task Should_Flag_Item_As_Removed()
        {
            using var scope = _serviceProvider.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<IItemService>();
            var repository = scope.ServiceProvider.GetRequiredService<IRepository<Item>>();

            var item = await repository.GetByIdAsync(ItemFake.IdManicure);

            item.Removed.Should().BeFalse();

            await service.RemoveItem(item.Id);

            var removedItem = await repository.GetByIdAsync(ItemFake.IdManicure);

            removedItem.Should().BeNull();
        }
    }
}
