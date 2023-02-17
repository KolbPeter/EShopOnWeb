using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using OrderDetailsDto = Microsoft.eShopWeb.ApplicationCore.DTOs.OrderDetails;

namespace DeliveryOrderProcessor.MongoDb.Collections;

public record OrderDetails : OrderDetailsDto, IEntity
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; init; }
}
