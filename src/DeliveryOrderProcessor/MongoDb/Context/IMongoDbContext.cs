using DeliveryOrderProcessor.MongoDb.Collections;
using MongoDB.Driver;

namespace DeliveryOrderProcessor.MongoDb.Context
{
    public interface IMongoDbContext
    {
        /// <summary>
        /// Gets all elements of a collection.
        /// </summary>
        IMongoCollection<OrderDetails> OrderDetails { get; }
    }
}
