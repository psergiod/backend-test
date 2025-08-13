using MongoDB.Bson;

namespace Salon.Infra.Tests.MongoDbContext
{
    public class DbContextFakes
    {
    }

    public class DataFake
    {
        public ObjectId Id { get; set; }
        public string Text { get; set; }
        public int Number { get; set; }
    }
}
