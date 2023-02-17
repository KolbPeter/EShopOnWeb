using DeliveryOrderProcessor.MongoDb.Collections;
using DeliveryOrderProcessor.MongoDb.Repositories;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using JsonConvert = Newtonsoft.Json.JsonConvert;

namespace DeliveryOrderProcessor
{
    public class AddOrderDetails
    {
        private readonly ILogger _logger;

        public AddOrderDetails(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<AddOrderDetails>();
        }

        [Function("AddOrderDetails")]
        public async Task<bool> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req)
        {
            _logger.LogInformation("HTTP trigger function 'AddOrderDetails' processed a request.");

            OrderDetails orderDetails;

            try
            {
                orderDetails = JsonConvert.DeserializeObject<OrderDetails>(await new StreamReader(req.Body).ReadToEndAsync());
            }
            catch (Exception ex)
            {
                _logger.LogError($"Deserialization failed with {ex}");
                return false;
            }

            try
            {

                var result = await new OrderDetailsRepository().CreateAsync(orderDetails);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Adding order details to database failed with {ex}");
                return false;
            }
        }
    }
}
