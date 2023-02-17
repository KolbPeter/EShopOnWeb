using DeliveryOrderProcessor.MongoDb.Collections;
using MongoDB.Driver;

namespace DeliveryOrderProcessor.MongoDb.Context
{
    public class MongoDbContext : IMongoDbContext
    {
        private const string connectionString = "mongodb://esow-mongodb:3aYR5UPNaMwt2enQdtTSbuE501ZrRJH2vn5JeYDJG2VcumBy15fk9MaqoBm2i33j77f5QHCZFbXAACDbzSDrCA==@esow-mongodb.mongo.cosmos.azure.com:10255/?ssl=true&replicaSet=globaldb&retrywrites=false&maxIdleTimeMS=120000&appName=@esow-mongodb@";
        private readonly IMongoDatabase database;

        /// <summary>
        /// Creates a Mongo db context.
        /// </summary>
        public MongoDbContext()
        {
            database = new MongoClient(connectionString).GetDatabase("OrderDetails");
        }

        /// <inheritdoc />
        public IMongoCollection<OrderDetails> OrderDetails =>
            database.GetCollection<OrderDetails>("OrderDetails");
    }
}
