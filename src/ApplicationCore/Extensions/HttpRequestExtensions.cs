using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.eShopWeb.ApplicationCore.Entities;

namespace Microsoft.eShopWeb.ApplicationCore.Extensions;
/// <summary>
/// Extension methods for <see cref="HttpRequest"/>
/// </summary>
public static class HttpRequestExtensions
{
    /// <summary>
    /// Sets the <see cref="HttpMethod"/> for this <see cref="HttpRequest"/>.
    /// </summary>
    /// <param name="httpRequest">The <see cref="HttpRequest"/> to modify.</param>
    /// <param name="method">The <see cref="HttpMethod"/> to set.</param>
    /// <returns>Returns a <see cref="HttpRequest"/> with the new value.</returns>
    public static HttpRequest SetMethod(this HttpRequest httpRequest, HttpMethod method) =>
        httpRequest with { Method = method };

    /// <summary>
    /// Sets the Url string for this <see cref="HttpRequest"/>.
    /// </summary>
    /// <param name="httpRequest">The <see cref="HttpRequest"/> to modify.</param>
    /// <param name="url">The url string to set.</param>
    /// <returns>Returns a <see cref="HttpRequest"/> with the new value.</returns>
    public static HttpRequest SetUrlString(this HttpRequest httpRequest, string url) =>
        httpRequest with { Url = url };

    /// <summary>
    /// Sets the <see cref="StringContent"/> for this <see cref="HttpRequest"/>.
    /// </summary>
    /// <param name="httpRequest">The <see cref="HttpRequest"/> to modify.</param>
    /// <param name="content">The <see cref="StringContent"/> to set.</param>
    /// <returns>Returns a <see cref="HttpRequest"/> with the new value.</returns>
    public static HttpRequest SetContent(this HttpRequest httpRequest, StringContent content) =>
        httpRequest with { Content = content };

    /// <summary>
    /// Adds the <see cref="KeyValuePair"/> for this <see cref="HttpRequest"/>.
    /// </summary>
    /// <param name="httpRequest">The <see cref="HttpRequest"/> to modify.</param>
    /// <param name="requestHeader">The <paramref name="requestHeader"/> to add.</param>
    /// <returns>Returns a <see cref="HttpRequest"/> with the new value.</returns>
    public static HttpRequest AddHeader(this HttpRequest httpRequest, KeyValuePair<string, string> requestHeader)
    {
        httpRequest.Headers.Add(requestHeader.Key, requestHeader.Value);
        return httpRequest;
    }

    /// <summary>
    /// Adds the given <paramref name="requestHeaders"/> for this <see cref="HttpRequest"/>.
    /// </summary>
    /// <param name="httpRequest">The <see cref="HttpRequest"/> to modify.</param>
    /// <param name="requestHeaders">The <paramref name="requestHeaders"/> to add.</param>
    /// <returns>Returns a <see cref="HttpRequest"/> with the new value.</returns>
    public static HttpRequest AddHeaders(this HttpRequest httpRequest, IDictionary<string, string>? requestHeaders) =>
        requestHeaders != null
            ? requestHeaders.Aggregate(httpRequest, (current, requestHeader) => current.AddHeader(requestHeader))
            : httpRequest;

    /// <summary>
    /// Awaitable. Sends the given <paramref name="httpRequest"/> to the given <see cref="HttpClient"/>.
    /// </summary>
    /// <param name="httpRequest">The <see cref="HttpRequest"/> to send.</param>
    /// <param name="httpClient">The <see cref="HttpClient"/> to send the request for.</param>
    /// <returns>Returns a value indicating whether the HTTP response was successful or not.</returns>
    public static async Task<bool> SendRequestAsync(
        this HttpRequest httpRequest,
        HttpClient httpClient)
    {
        var result = await httpClient
            .SendAsync(httpRequest.ConvertToHttpRequestMessage());

        return result.IsSuccessStatusCode;
    }

    private static HttpRequestMessage ConvertToHttpRequestMessage(this HttpRequest httpRequest)
    {
        var result = new HttpRequestMessage
        {
            Method = httpRequest.Method,
            RequestUri = new Uri(httpRequest.Url),
            Content = httpRequest.Content
        };

        foreach (var httpRequestHeader in httpRequest.Headers)
        {
            result.Headers.Add(name: httpRequestHeader.Key, value: httpRequestHeader.Value);
        }

        return result;
    }
}
