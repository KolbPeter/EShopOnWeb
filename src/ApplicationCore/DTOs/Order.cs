using System;
using System.Collections.Generic;

namespace Microsoft.eShopWeb.ApplicationCore.DTOs;

public record Order
{
    public int Id { get; init; }
    public string BuyerId { get; init; }
    public DateTimeOffset OrderDate { get; init; }
    public Address ShipToAddress { get; init; }
    public IEnumerable<OrderItem> OrderItems { get; init; }
}
