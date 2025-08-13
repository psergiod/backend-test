using MongoDB.Bson;

namespace Salon.Domain.Extensions
{
    public static class StringExtensions
    {
        public static bool IsValidObjectId(this string id)
        {
            return ObjectId.TryParse(id.ToString(), out _);
        }
    }
}
