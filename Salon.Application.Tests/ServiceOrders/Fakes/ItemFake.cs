using MongoDB.Bson;
using Salon.Domain.ServiceOrders.Contracts;
using ItemEntity = Salon.Domain.ServiceOrders.Entities.Item;

namespace Salon.Application.Tests.ServiceOrders.Fakes
{
    public static class ItemFake
    {
        public static ObjectId IdBrazilianBlowout = ObjectId.Parse("63e5cdeebf4932a2d1e8ba81");
        public const string DESCRIPTION_BRAZILIAN_BLOWOUT = "Brazilian Blowout";

        public static ObjectId IdManicure = ObjectId.Parse("649205a22524c80b79e0d19d");
        public const string DESCRIPTION_MANICURE = "Manicure";

        public const string DESCRIPTION_PEDICURE = "Pedicure";
        public static ItemEntity GetItemEntity(string description, ObjectId? id = null)
        {
            var item = new ItemEntity()
            {
                Description = description
            };

            item.Id = id ?? ObjectId.Empty;

            return item;
        }

        public static ItemCommand GetItemCommandPedicure()
            => new ItemCommand()
            {
                Description = DESCRIPTION_PEDICURE
            };

        public static ItemResponse GetItemResponseManicure()
            => new ItemResponse()
            {
                Description = DESCRIPTION_MANICURE,
                Id = IdManicure.ToString()
            };

        public static ItemResponse GetItemResponseBrazilianBlowout()
    => new ItemResponse()
    {
        Description = DESCRIPTION_BRAZILIAN_BLOWOUT,
        Id = IdBrazilianBlowout.ToString()
    };

        public static ItemEntity GetItemBrazilianBlowout()
            => GetItemEntity(DESCRIPTION_BRAZILIAN_BLOWOUT, IdBrazilianBlowout);

        public static ItemEntity GetItemManicure()
            => GetItemEntity(DESCRIPTION_MANICURE, IdManicure);

        public static ItemEntity GetItemPedicure(ObjectId id)
            => GetItemEntity(DESCRIPTION_PEDICURE, id);
    }
}
