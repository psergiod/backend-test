using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Driver;
using Salon.Domain.Base;
using Salon.Domain.Clients.Entities;
using Salon.Domain.Clients.Repositories;
using Salon.Domain.Constants;
using Salon.Domain.Models;
using Salon.Domain.Models.Enums;
using Salon.Domain.ServiceOrders.Entities;
using Salon.Domain.ServiceOrders.Repositories;
using Salon.Domain.Users.Entities;
using Salon.Domain.Users.Repositories;
using Salon.Infra.CollectionDefinitions;
using Salon.Infra.DbContext;
using Salon.Infra.Repositories;
using System;

namespace Salon.Infra
{
    public static class InfraExtensions
    {
        public static IServiceCollection AddMongoDbDependencyInjection(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.AddSingleton<IMongoClient, MongoClient>(sp =>
            {
                var clientSettings = MongoClientSettings.FromConnectionString(configuration.GetConnectionString(ConnectionStringsConstants.MONGODB));
                clientSettings.LinqProvider = MongoDB.Driver.Linq.LinqProvider.V3;
                return new MongoClient(clientSettings);
            });

            return serviceCollection;
        }

        public static IServiceCollection AddCollectionDefinitions(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<ICollectionDefinitions<Client>, ClientDefinitions>();
            serviceCollection.AddTransient<ICollectionDefinitions<User>, UserDefinitions>();

            return serviceCollection;
        }
        public static IServiceCollection AddInfraDependencyInjection(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IMongoDbContext, MongoDbContext>();
            serviceCollection.AddTransient<IClientRepository, ClientRepository>();
            serviceCollection.AddTransient<IUserRepository, UserRepository>();
            serviceCollection.AddTransient<IClientRemovedRepository>(x => new ClientRepository(
                x.GetRequiredService<IMongoDbContext>(),
                true));

            serviceCollection.AddTransient<IUserRemovedRepository>(x => new UserRepository(
                x.GetRequiredService<IMongoDbContext>(),
                true));

            serviceCollection.AddTransient<IServiceOrderRepository, ServiceOrderRepository>();
            serviceCollection.AddTransient<IRepository<Item>, Repository<Item>>();

            CreateAdminUser(serviceCollection.BuildServiceProvider());
            serviceCollection.AddTransient<IServiceBuilderBase<Entity<ObjectId>>, ServiceBuilderBase<Entity<ObjectId>>>();

            return serviceCollection;
        }

        private static void CreateAdminUser(IServiceProvider serviceProvider)
        {
            var user = new User("admin", BCrypt.Net.BCrypt.HashPassword("admin"), "admin")
                .InformEmail("admin@gmail.com")
                .InformRole(Role.Admin);

            var repository = serviceProvider.GetRequiredService<IUserRepository>();

            if (repository != null && repository.GetUserByLoginAsync(user.Login).Result == null)
            {
                repository.InsertAsync(user);
                Console.WriteLine("Usuario admin adicionado a base de dados.");
            }
        }
    }
}
