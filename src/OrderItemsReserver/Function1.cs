using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace OrderItemsReserver
{
    public class Function1
    {
        private readonly ILogger _logger;

        public Function1(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<Function1>();
        }

        [Function("Function1")]
        public void Run([ServiceBusTrigger("neworder")] string myQueueItem)
        {
            _logger.LogInformation($"C# ServiceBus queue trigger function processed message: {myQueueItem}");
        }
    }
}
