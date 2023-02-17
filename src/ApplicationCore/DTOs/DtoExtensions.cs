using System.Linq;
using AddressEntity = Microsoft.eShopWeb.ApplicationCore.Entities.OrderAggregate.Address;
using CatalogItemOrderedEntity = Microsoft.eShopWeb.ApplicationCore.Entities.OrderAggregate.CatalogItemOrdered;
using OrderEntity = Microsoft.eShopWeb.ApplicationCore.Entities.OrderAggregate.Order;
using OrderItemEntity = Microsoft.eShopWeb.ApplicationCore.Entities.OrderAggregate.OrderItem;

namespace Microsoft.eShopWeb.ApplicationCore.DTOs;

public static class DtoExtensions
{
    public static OrderEntity ToEntity(this Order order) =>
        new (
            id: order.Id,
            buyerId: order.BuyerId,
            shipToAddress: order.ShipToAddress.ToEntity(),
            items: order.OrderItems.Select(x => x.ToEntity()).ToList());

    public static AddressEntity ToEntity(this Address address) =>
        new (
            street: address.Street,
            city: address.City,
            state: address.State,
            country: address.Country,
            zipcode: address.ZipCode);

    public static OrderItemEntity ToEntity(this OrderItem item) =>
        new (
            id: item.Id,
            itemOrdered: item.ItemOrdered.ToEntity(),
            unitPrice: item.UnitPrice,
            units: item.Units);

    public static CatalogItemOrderedEntity ToEntity(this CatalogItemOrdered item) =>
        new (
            catalogItemId: item.CatalogItemId,
            productName: item.ProductName,
            pictureUri: item.PictureUri);

    public static OrderDetails ToOrderDetails(this OrderEntity order) =>
        new()
        {
            OrderItems = order.OrderItems.Select(x => x.ToDto()).ToArray(),
            ShipToAddress = order.ShipToAddress.ToDto(),
            Total = order.Total()
        };

    public static Address ToDto(this AddressEntity address) =>
        new ()
        {
            Street = address.Street,
            City = address.City,
            State = address.State,
            Country = address.Country,
            ZipCode = address.ZipCode
        };

    public static OrderItem ToDto(this OrderItemEntity item) =>
        new()
        {
            Id = item.Id,
            ItemOrdered = item.ItemOrdered.ToDto(),
            UnitPrice = item.UnitPrice,
            Units = item.Units
        };

    public static CatalogItemOrdered ToDto(this CatalogItemOrderedEntity item) =>
        new()
        {
            CatalogItemId = item.CatalogItemId,
            ProductName = item.ProductName,
            PictureUri = item.PictureUri
        };
}
