using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace StockParser.NoSql.Models
{
    public class BaseMongoModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
    }
}
