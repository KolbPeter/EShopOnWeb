using System;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using Microsoft.Extensions.Logging;

namespace Microsoft.eShopWeb.ApplicationCore.Services;

public class ServiceBusQueueService : IServiceBusQueueService
{
    private readonly ILogger<ServiceBusQueueService> _logger;

    public ServiceBusQueueService(ILogger<ServiceBusQueueService> logger)
    {
        _logger = logger;
    }

    public async Task<bool> PushMessage(
        string connectionString,
        string queueName,
        string message)
    {
        var options = new ServiceBusClientOptions();
        options.RetryOptions = new ServiceBusRetryOptions
        {
            Delay = TimeSpan.FromSeconds(10),
            MaxDelay = TimeSpan.FromSeconds(30),
            Mode = ServiceBusRetryMode.Exponential,
            MaxRetries = 3,
        };
        await using var client = new ServiceBusClient(connectionString, options);

        try
        {
            await client.CreateSender(queueName).SendMessageAsync(new ServiceBusMessage(message));
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return false;
        }
    }
}
