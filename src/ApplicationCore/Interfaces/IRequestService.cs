using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.eShopWeb.ApplicationCore.DTOs;

namespace Microsoft.eShopWeb.ApplicationCore.Interfaces;

public interface IRequestService
{
    Task<bool> RequestToAddDeliveryOrderDetails(OrderDetails orderDetails, IDictionary<string, string>? requestHeaders = null);
}
