using Microsoft.Azure.Functions.Worker;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using Microsoft.Extensions.Logging;

namespace OrderItemsReserver
{
    public class AddOrderBlob
    {
        private readonly IStorage _blobStorage;
        private readonly ILogger _logger;

        public AddOrderBlob(
            ILoggerFactory loggerFactory,
            IStorage blobStorage)
        {
            _blobStorage = blobStorage;
            _logger = loggerFactory.CreateLogger<AddOrderBlob>();
        }

        [Function("AddOrderBlob")]
        public async Task Run(
            [ServiceBusTrigger("neworder")] string order)
        {
            _logger.LogInformation($"ServiceBus queue trigger function processed message: {order}");

            await _blobStorage.ReserveOrderAsync(order);
        }
    }
}
