using DeliveryOrderProcessor.MongoDb.Collections;
using DeliveryOrderProcessor.MongoDb.Context;

namespace DeliveryOrderProcessor.MongoDb.Repositories
{
    public class OrderDetailsRepository : GenericMongoDbRepository<OrderDetails>
    {
        public OrderDetailsRepository() : base(new MongoDbContext().OrderDetails)
        {
        }
    }
}
