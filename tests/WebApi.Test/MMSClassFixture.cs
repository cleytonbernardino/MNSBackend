using MMS.Domain.ValueObjects;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace WebApi.Test;

public abstract class MmsClassFixture(CustomWebApplicationFactory factory) : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client = factory.CreateClient();
    
    protected abstract string Method { get; }
    
    protected async Task<HttpResponseMessage> DoPostAsync(object request, string token = "", string culture = "en")
    {
        ChangeRequestCulture(culture);
        AuthorizeRequest(token);
        return await _client.PostAsJsonAsync(Method, request);
    }
    
    protected async Task<HttpResponseMessage> DoGetAsync(
        string token = "", string culture = "en", string? customUrl = null)
    {
        ChangeRequestCulture(culture);
        AuthorizeRequest(token);
        return await _client.GetAsync(customUrl ?? Method);
    }
    
    protected async Task<HttpResponseMessage> DoGetWithRefreshTokenAsync(
        string refreshToken, string accessToken = "", string culture = "en"
    )
    {
        ChangeRequestCulture(culture);
        AuthorizeRequest(accessToken);
        _client.DefaultRequestHeaders.Add("Cookie", $"{MMSConst.REFRESH_TOKEN_COOKIE_KEY}={refreshToken}");
        return await _client.GetAsync(Method);
    }

    protected async Task<HttpResponseMessage> DoPutAsync( object request, string token, string culture = "en")
    {
        ChangeRequestCulture(culture);
        AuthorizeRequest(token);
        return await _client.PutAsJsonAsync(Method, request);
    }

    protected async Task<HttpResponseMessage> DoDeleteAsync(
        string token, string culture = "en", string? customUrl = null)
    {
        ChangeRequestCulture(culture);
        AuthorizeRequest(token);
        return await _client.DeleteAsync(customUrl ?? Method);
    }
    
    private void AuthorizeRequest(string? token)
    {
        if (string.IsNullOrWhiteSpace(token))
            return;

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    private void ChangeRequestCulture(string culture)
    {
        if (_client.DefaultRequestHeaders.Contains("Accept-Language"))
            _client.DefaultRequestHeaders.Remove("Accept-Language");
        _client.DefaultRequestHeaders.Add("Accept-Language", culture);
    }
}
