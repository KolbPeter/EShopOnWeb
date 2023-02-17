namespace Microsoft.eShopWeb.ApplicationCore.DTOs;

public record CatalogItemOrdered
{
    public int CatalogItemId { get; init; }
    public string ProductName { get; init; }
    public string PictureUri { get; init; }
}
