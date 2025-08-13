using Microsoft.Extensions.DependencyInjection;
using Salon.Domain.Clients.Entities;
using Salon.Domain.Users.Entities;
using System;

namespace Salon.Infra.CollectionDefinitions
{
    public static class CollectionDefinitionsBuilder
    {
        public static void BuildIndexes(this IServiceCollection serviceColletion)
        {
            Console.WriteLine("Building Collection's indexes...");

            var serviceProvider = serviceColletion.BuildServiceProvider();
            serviceProvider.GetRequiredService<ICollectionDefinitions<User>>().BuildIndexes();
            serviceProvider.GetRequiredService<ICollectionDefinitions<Client>>().BuildIndexes();
        }
    }
}
