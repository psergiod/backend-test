using Salon.Application.Tests.DataLoad;
using Salon.Application.Tests.ServiceOrders.Fakes;
using Salon.Domain.Base;
using Salon.Domain.ServiceOrders.Entities;
using System.Threading.Tasks;

namespace Salon.Application.Tests.ServiceOrders.Loads
{
    public class ItemDataLoad : IDataLoad<Item>
    {
        private readonly IRepository<Item> _repository;

        public ItemDataLoad(IRepository<Item> repository)
        {
            _repository = repository;
        }

        public async Task LoadAsync()
        {
            await _repository.InsertAsync(ItemFake.GetItemBrazilianBlowout());
            await _repository.InsertAsync(ItemFake.GetItemManicure());
        }

        public async Task<bool> IsLoadedAsync()
        {
            return await _repository.GetByIdAsync(ItemFake.IdBrazilianBlowout) != null &&
                await _repository.GetByIdAsync(ItemFake.IdManicure) != null;
        }
    }
}
