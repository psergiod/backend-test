using MongoDB.Bson;
using Salon.Domain.Models;
using System.Threading.Tasks;

namespace Salon.Application.Tests.DataLoad
{
    public interface IDataLoad<TEntity> where TEntity : IEntity<ObjectId>
    {
        Task LoadAsync();
        Task<bool> IsLoadedAsync();
    }
}
