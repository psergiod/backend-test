using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using Salon.Domain.Models;
using Salon.Domain.Users.Entities;
using System;
using System.Threading.Tasks;

namespace Salon.Application.Tests.DataLoad
{
    public class DataLoadBuilder
    {
        private readonly IServiceProvider _serviceProvider;

        public DataLoadBuilder(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<DataLoadBuilder> AddLoadAsync<TEntity>() where TEntity: Entity<ObjectId>
        {
            var dataLoad = _serviceProvider.GetService<IDataLoad<TEntity>>();
            bool isLoaded = await dataLoad.IsLoadedAsync();

            while (!isLoaded)
            {
                await dataLoad.LoadAsync();
                isLoaded = await dataLoad.IsLoadedAsync();
            }

            return this;
        }
    }
}
