using Salon.Domain.ServiceOrders.Contracts;
using Salon.Domain.ServiceOrders.Entities;
using System.Collections.Generic;

namespace Salon.Application.ServiceOrders.Interfaces
{
    public interface IItemMapper
    {
        ItemResponse MapResponse(Item item);
        List<ItemResponse> MapResponse(List<Item> items);
    }
}