using FluentAssertions;
using MongoDB.Driver;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace Salon.Infra.Tests.MongoDbContext
{
    [TestFixture]
    public class MongoDbContextTest
    {
        private string DB_NAME = $"db_test{Guid.NewGuid()}";
        private const string COLLECTION_NAME = "TestCollection";
        private DbFixture _dbFixture;

        [OneTimeSetUp]
        public void SetUp()
        {
            _dbFixture = new DbFixture(DB_NAME);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _dbFixture.Dispose();
        }

        [Test]
        public async Task Should_Create_New_Collection_And_Save_When_Receive_New_Data()
        {
            var data = new DataFake { Text = "test", Number = 5 };
            await LoadCollectionWithDataFake(data);
            var collection = _dbFixture.DbContext.GetCollection<DataFake>(COLLECTION_NAME);

            var filter = Builders<DataFake>.Filter.Eq(x => x.Number, 5);
            var expected = collection.Find(filter).FirstOrDefault();

            expected.Should().NotBeNull();
            expected.Id.Should().NotBeNull();
            expected.Should().BeEquivalentTo(data);
        }

        [Test]
        public async Task Should_Remove_Collection()
        {
            var data = new DataFake { Text = "testRemove", Number = 4 };
            await LoadCollectionWithDataFake(data);

            var collection = _dbFixture.DbContext.GetCollection<DataFake>(COLLECTION_NAME);

            var filter = Builders<DataFake>.Filter.Eq(x => x.Number, 4);
            var created = collection.Find(filter).FirstOrDefault();

            created.Should().BeEquivalentTo(data);

            await collection.DeleteOneAsync(filter);

            var expected = collection.Find(filter).FirstOrDefault();
            expected.Should().BeNull();
        }

        private async Task LoadCollectionWithDataFake(DataFake data)
        {
            var collection = _dbFixture.DbContext.GetCollection<DataFake>(COLLECTION_NAME);
            await collection.InsertOneAsync(data);
        }
    }
}
