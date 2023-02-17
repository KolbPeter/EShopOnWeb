namespace Microsoft.eShopWeb.ApplicationCore.DTOs;

public record OrderItem
{
    public int Id { get; init; }
    public CatalogItemOrdered ItemOrdered { get; init; }
    public decimal UnitPrice { get; init; }
    public int Units { get; init; }
}
