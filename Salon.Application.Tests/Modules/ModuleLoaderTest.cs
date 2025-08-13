using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Salon.Application.Tests.DataLoad;
using Salon.Application.Tests.Users.Fakes;
using Salon.Domain.Users.Entities;
using Salon.Domain.Users.Repositories;
using Salon.Infra.DbContext;
using System;
using System.Threading.Tasks;

namespace Salon.Application.Tests.Modules
{
    [TestFixture]
    public class ModuleLoaderTest
    {
        private IServiceProvider _serviceProvider;
        private IMongoDbContext _mongoDbContext;

        [OneTimeSetUp]
        public async Task SetUp()
        {
            var serviceCollection = new ServiceCollection();
            //serviceCollection.AddScoped(_ => _dbFixture.MongoClient);

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
        }

        [Test]
        public async Task Should_Load_Data_From_Module()
        {
            var scope = _serviceProvider.CreateScope();

            var repository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
            var entity = await repository.GetByIdAsync(UserFake.IdFoo);
            entity.Should().NotBeNull();
            entity.Name.Should().Be(UserFake.NAME_FOO);
        }
    }
}
