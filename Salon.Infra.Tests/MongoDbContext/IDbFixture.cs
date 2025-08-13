using System;

namespace Salon.Infra.Tests.MongoDbContext
{
    public interface IDbFixture
    {
        void Dispose();
    }
}