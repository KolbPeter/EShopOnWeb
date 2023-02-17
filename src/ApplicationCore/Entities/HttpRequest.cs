using System.Collections.Generic;
using System.Net.Http;

namespace Microsoft.eShopWeb.ApplicationCore.Entities;

/// <summary>
/// Record to store HTTP request message properties.
/// </summary>
public record HttpRequest
{
    /// <summary>
    /// Gets the Url string for this request.
    /// </summary>
    public string Url { get; init; } = string.Empty;

    /// <summary>
    /// Gets the <see cref="HttpMethod"/> to use for this request.
    /// </summary>
    public HttpMethod Method { get; init; } = HttpMethod.Get;

    /// <summary>
    /// Gets the <see cref="StringContent"/> for this request.
    /// </summary>
    public StringContent? Content { get; init; } = null;

    /// <summary>
    /// Gets a <see cref="IDictionary{TKey,TValue}"/> that contains the request headers.
    /// </summary>
    public IDictionary<string, string> Headers { get; init; } = new Dictionary<string, string>();
}
