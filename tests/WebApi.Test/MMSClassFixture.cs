using MMS.Domain.ValueObjects;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace WebApi.Test;

public class MmsClassFixture(CustomWebApplicationFactory factory) : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client = factory.CreateClient();
    
    protected async Task<HttpResponseMessage> DoPostAsync(string method, object request, string token = "", string culture = "en")
    {
        ChangeRequestCulture(culture);
        AuthorizeRequest(token);
        return await _client.PostAsJsonAsync(method, request);
    }
    
    protected async Task<HttpResponseMessage> DoPostWithRefreshTokenAsync(
        string method, object request, string refreshToken, string culture = "en"
        )
    {
        ChangeRequestCulture(culture);
        _client.DefaultRequestHeaders.Add("Cookie", $"{MMSConst.REFRESH_TOKEN_COOKIE_KEY}={refreshToken}");
        return await _client.PostAsJsonAsync(method, request);
    }
    
    protected async Task<HttpResponseMessage> DoGetAsync(string method, string token = "", string culture = "en")
    {
        ChangeRequestCulture(culture);
        AuthorizeRequest(token);
        return await _client.GetAsync(method);
    }

    protected async Task<HttpResponseMessage> DoPutAsync(string method, object request, string token, string culture = "en")
    {
        ChangeRequestCulture(culture);
        AuthorizeRequest(token);
        return await _client.PutAsJsonAsync(method, request);
    }

    protected async Task<HttpResponseMessage> DoDeleteAsync(string method, string token, string culture = "en")
    {
        ChangeRequestCulture(culture);
        AuthorizeRequest(token);
        return await _client.DeleteAsync(method);
    }
    
    private void AuthorizeRequest(string? token)
    {
        if (string.IsNullOrWhiteSpace(token))
            return;

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    /// <summary>
    /// Reads the body of an HTTP response, parses it as JSON, and returns the root <see cref="JsonElement"/>.
    /// </summary>
    /// <param name="response">The HTTP response containing the JSON content.</param>
    /// <returns>
    /// The root <see cref="JsonElement"/> of the parsed JSON document.
    /// </returns>
    /// <remarks>
    /// The returned <see cref="JsonElement"/> is tied to the lifetime of the underlying <see cref="JsonDocument"/>.
    /// Avoid disposing the document before you finish accessing the element. 
    /// If a standalone object is required, consider cloning the element using <c>JsonDocument.Parse(element.GetRawText())</c>.
    /// </remarks>
    protected static async Task<JsonElement> GetRootElement(HttpResponseMessage response)
    {
        await using Stream responseBody = await response.Content.ReadAsStreamAsync();
        var responseData = await JsonDocument.ParseAsync(responseBody);
        return responseData.RootElement;
    }


    /// <summary>
    /// Retrieves an enumerator for the elements of an array property within the root JSON element
    /// of an HTTP response.
    /// </summary>
    /// <param name="response">The HTTP response containing the JSON content.</param>
    /// <returns>
    /// A <see cref="JsonElement.ArrayEnumerator"/> to iterate over the elements of the specified array property.
    /// </returns>
    /// <remarks>
    /// This method uses <see cref="GetRootElement(HttpResponseMessage)"/> to obtain the root element
    /// and directly accesses the specified property. 
    /// It will throw a <see cref="KeyNotFoundException"/> if the property does not exist.
    /// Consider adding validation with <c>TryGetProperty</c> to avoid exceptions.
    /// </remarks>
    protected static async Task<JsonElement.ArrayEnumerator> GetArrayFromResponse(HttpResponseMessage response, string key = "errors")
    {
        var jsonElement = await GetRootElement(response);
        return jsonElement.GetProperty(key).EnumerateArray();
    }

    private void ChangeRequestCulture(string culture)
    {
        if (_client.DefaultRequestHeaders.Contains("Accept-Language"))
            _client.DefaultRequestHeaders.Remove("Accept-Language");
        _client.DefaultRequestHeaders.Add("Accept-Language", culture);
    }
}
