using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.eShopWeb.ApplicationCore.DTOs;
using Microsoft.eShopWeb.ApplicationCore.Entities;
using Microsoft.eShopWeb.ApplicationCore.Extensions;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;

namespace Microsoft.eShopWeb.ApplicationCore.Services;
public class RequestService : IRequestService
{
    private readonly string _deliveryProcessorUrl;
    private readonly IDictionary<string, string> _deaultHeaders;
    private readonly HttpClient _client;
    private readonly JsonSerializerOptions _serializerOptions = new() { PropertyNameCaseInsensitive = true };

    public RequestService(
        string deliveryProcessorUrl,
        IDictionary<string, string> deaultHeaders)
    {
        _deliveryProcessorUrl = deliveryProcessorUrl;
        _deaultHeaders = deaultHeaders;
        _client = new HttpClient();
    }

    public async Task<bool> RequestToAddDeliveryOrderDetails(
        OrderDetails orderDetails,
        IDictionary<string, string>? requestHeaders = null)
    {
        return await new HttpRequest()
            .AddHeaders(_deaultHeaders)
            .AddHeaders(requestHeaders)
            .SetMethod(HttpMethod.Post)
            .SetContent(new StringContent(JsonSerializer.Serialize(orderDetails, _serializerOptions)))
            .SetUrlString(UrlString(_deliveryProcessorUrl, "/api/AddOrderDetails"))
            .SendRequestAsync(httpClient: _client);
    }

    private string UrlString(
        string url,
        string route,
        IDictionary<string, string>? queries = null)
    {
        var stuff = $"{url}{route}{QueryString(queries)}";
        return $"{url}{route}{QueryString(queries)}";
    }

    private static string QueryString(IDictionary<string, string>? queries) =>
        queries != null
            ? $"?{string.Join("&", queries.Select(x => $"{x.Key}={x.Value}").ToArray())}"
            : string.Empty;
}
