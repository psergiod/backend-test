using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using NSubstitute;
using NUnit.Framework;
using Salon.Domain.Users.Entities;
using Salon.Infra.CollectionDefinitions;
using Salon.Infra.DbContext;
using Salon.Infra.Tests.MongoDbContext;
using System;

namespace Salon.Infra.Tests.CollectionDefinitions
{
    [TestFixture]
    public class CollectionDefinitionsTest
    {
        private string DB_NAME = $"db_test{Guid.NewGuid()}";
        private DbFixture _dbFixture;
        private ServiceCollection _serviceCollection;
        private IMongoDbContext _mongoDbContext;

        [OneTimeSetUp]
        public void SetUp()
        {
            _dbFixture = new DbFixture(DB_NAME);

            _serviceCollection = new ServiceCollection();

            var configurationFake = Substitute.For<IConfiguration>();
            configurationFake[Arg.Any<string>()].Returns(DB_NAME);

            _mongoDbContext = new DbContext.MongoDbContext(_dbFixture.MongoClient, configurationFake);

            _serviceCollection.AddScoped(_ => _dbFixture.MongoClient);
            _serviceCollection.AddScoped(_ => _mongoDbContext);

            _serviceCollection.AddTransient<ICollectionDefinitions<User>, UserDefinitions>();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _dbFixture.Dispose();
        }

        [Test]
        public void Should_Load_CollectionDefinitions_Of_User_Collection()
        {
            var serviceProvider = _serviceCollection.BuildServiceProvider();

            serviceProvider.GetRequiredService<ICollectionDefinitions<User>>().BuildIndexes();

            var coll = _mongoDbContext.GetCollection<User>(typeof(User).Name);
            var hasIndexes = coll.Indexes.List().ToList().Count;

            hasIndexes.Should().BeGreaterThan(1);
        }
    }
}
