using System.Threading.Tasks;

namespace Microsoft.eShopWeb.ApplicationCore.Interfaces;

public interface IServiceBusQueueService
{
    Task<bool> PushMessage(string connectionString, string queueName,string message);
}
