using System.Collections.Generic;

namespace Microsoft.eShopWeb.ApplicationCore.DTOs;

public record OrderDetails
{
    public Address ShipToAddress { get; init; }
    public IEnumerable<OrderItem> OrderItems { get; init; }
    public decimal Total { get; init; }
}
