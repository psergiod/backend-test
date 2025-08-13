using MongoDB.Bson;
using Salon.Domain.Models;

namespace Salon.Domain.ServiceOrders.Entities
{
    public class ItemOrder
    {
        public ObjectId ItemId { get; set; }
        public decimal Value { get; set; }
    }
}
