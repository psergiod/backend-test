using MongoDB.Bson;
using Salon.Domain.Models;

namespace Salon.Domain.ServiceOrders.Entities
{
    public class Item : Entity<ObjectId>
    {
        public string Description { get; set; }
    }
}
