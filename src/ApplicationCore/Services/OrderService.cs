using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Microsoft.eShopWeb.ApplicationCore.DTOs;
using Microsoft.eShopWeb.ApplicationCore.Entities;
using Microsoft.eShopWeb.ApplicationCore.Entities.BasketAggregate;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using Microsoft.eShopWeb.ApplicationCore.Specifications;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Address = Microsoft.eShopWeb.ApplicationCore.Entities.OrderAggregate.Address;
using CatalogItemOrdered = Microsoft.eShopWeb.ApplicationCore.Entities.OrderAggregate.CatalogItemOrdered;
using Order = Microsoft.eShopWeb.ApplicationCore.Entities.OrderAggregate.Order;
using OrderItem = Microsoft.eShopWeb.ApplicationCore.Entities.OrderAggregate.OrderItem;

namespace Microsoft.eShopWeb.ApplicationCore.Services;

public class OrderService : IOrderService
{
    private readonly JsonSerializerOptions _serializerOptions = new() { PropertyNameCaseInsensitive = true };
    private readonly IRepository<Order> _orderRepository;
    private readonly IUriComposer _uriComposer;
    private readonly IServiceBusQueueService _serviceBusQueueService;
    private readonly IRequestService _requestService;
    private readonly ILogger<OrderService> _logger;
    private readonly IConfiguration _configuration;
    private readonly IRepository<Basket> _basketRepository;
    private readonly IRepository<CatalogItem> _itemRepository;

    public OrderService(IRepository<Basket> basketRepository,
        IRepository<CatalogItem> itemRepository,
        IRepository<Order> orderRepository,
        IUriComposer uriComposer,
        IServiceBusQueueService serviceBusQueueService,
        IRequestService requestService,
        ILogger<OrderService> logger,
        IConfiguration configuration)
    {
        _orderRepository = orderRepository;
        _uriComposer = uriComposer;
        _serviceBusQueueService = serviceBusQueueService;
        _requestService = requestService;
        _logger = logger;
        _configuration = configuration;
        _basketRepository = basketRepository;
        _itemRepository = itemRepository;
    }

    public async Task CreateOrderAsync(int basketId, Address shippingAddress)
    {
        var basketSpec = new BasketWithItemsSpecification(basketId);
        var basket = await _basketRepository.FirstOrDefaultAsync(basketSpec);

        Guard.Against.Null(basket, nameof(basket));
        Guard.Against.EmptyBasketOnCheckout(basket.Items);

        var catalogItemsSpecification = new CatalogItemsSpecification(basket.Items.Select(item => item.CatalogItemId).ToArray());
        var catalogItems = await _itemRepository.ListAsync(catalogItemsSpecification);

        var items = basket.Items.Select(basketItem =>
        {
            var catalogItem = catalogItems.First(c => c.Id == basketItem.CatalogItemId);
            var itemOrdered = new CatalogItemOrdered(catalogItem.Id, catalogItem.Name, _uriComposer.ComposePicUri(catalogItem.PictureUri));
            var orderItem = new OrderItem(itemOrdered, basketItem.UnitPrice, basketItem.Quantity);
            return orderItem;
        }).ToList();

        var order = new Order(basket.BuyerId, shippingAddress, items);

        var placedOrder = await _orderRepository.AddAsync(order);

        var orderMessage = JsonSerializer.Serialize(placedOrder, _serializerOptions);
        await _serviceBusQueueService.PushMessage(
            connectionString: _configuration["ServiceBusConnectionString:ServiceBusConnectionString"],
            queueName: _configuration["ServiceBusConnectionString:ServiceBusQueueName"],
            message: orderMessage);

        var orderDetail = placedOrder.ToOrderDetails();
        if (await _requestService.RequestToAddDeliveryOrderDetails(orderDetail, null))
        {
            _logger.LogDebug("Add order details for delivery was successful!");
        }
        else
        {
            _logger.LogError("Add order details for delivery failed!");
        }
    }
}
