using Salon.Application.ServiceOrders.Interfaces;
using Salon.Domain.ServiceOrders.Contracts;
using Salon.Domain.ServiceOrders.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Salon.Application.ServiceOrders.Mappers
{
    public class ItemMapper : IItemMapper
    {
        public ItemResponse MapResponse(Item item)
            => new ItemResponse
            {
                Id = item?.Id.ToString(),
                Description = item?.Description
            };

        public List<ItemResponse> MapResponse(List<Item> items)
            => items.Select(x => MapResponse(x)).ToList();
    }
}
