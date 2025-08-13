using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Salon.Application;
using Salon.Domain.Users.Entities;
using Salon.Infra;
using Salon.Infra.CollectionDefinitions;
using Salon.Infra.Tests.MongoDbContext;
using System;
using System.IO;
using System.Reflection;

namespace Salon.Application.Tests.Modules
{
    public class StartupModule : IModule
    {
        public ServiceCollection Register(ServiceCollection service)
        {
            return AddStartupDependencies(service);
        }
        public ServiceCollection AddStartupDependencies(ServiceCollection service)
        {
            var test = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();

            service.AddTransient<IConfiguration>(x => configuration);

            service.AddMongoDbDependencyInjection(configuration);
            service.AddInfraDependencyInjection();
            service.AddCollectionDefinitions();
            service.AddAppDependencyInjection();
            service.AddTransient<IDbFixture, DbFixture>();

            service.BuildIndexes();

            return service;
        }
    }
}
