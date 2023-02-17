using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.eShopWeb.ApplicationCore.Entities.OrderAggregate;

namespace Microsoft.eShopWeb.ApplicationCore.Interfaces;

public interface IStorage
{
    Task ReserveOrderAsync(string jsonString);
    Task<IEnumerable<string>> GetOrderNamesAsync();
    Task<IEnumerable<Order>> GetOrdersAsync();
}
