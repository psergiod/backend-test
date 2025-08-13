using MongoDB.Bson;
using Salon.Application.ServiceOrders.Interfaces;
using Salon.Domain.Base;
using Salon.Domain.ServiceOrders.Contracts;
using Salon.Domain.ServiceOrders.Entities;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Salon.Application.ServiceOrders.Services
{
    public class ItemService : IItemService
    {
        private readonly IRepository<Item> _itemRepository;
        private readonly IItemMapper _itemMapper;
        public ItemService(IRepository<Item> itemRepository, IItemMapper itemMapper)
        {
            _itemRepository = itemRepository;
            _itemMapper = itemMapper;
        }

        public async Task<Result> CreateItem(ItemCommand item)
        {
            var entity = new Item() { Description = item.Description };

            await _itemRepository.InsertAsync(entity);

            return new Result(HttpStatusCode.Created);
        }

        public async Task<Result> GetItemById(ObjectId id)
        {
            var item = await _itemRepository.GetByIdAsync(id);

            return new Result(_itemMapper.MapResponse(item), HttpStatusCode.OK);
        }

        public async Task<Result> GetAllAsync()
        {
            var items = await _itemRepository.GetAllAsync();

            return new Result(_itemMapper.MapResponse(items.ToList()), HttpStatusCode.OK);
        }

        public async Task<Result> RemoveItem(ObjectId id)
        {
            await _itemRepository.RemoveAsync(id);

            return new Result(HttpStatusCode.OK);
        }
    }
}
