using System.Globalization;

namespace MMS.API.Middleware;

internal class CultureMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    public async Task Invoke(HttpContext context)
    {
        var supportedLanguages = CultureInfo.GetCultures(CultureTypes.AllCultures);
        CultureInfo cultureInfo = new("en");

        string? requestCulture = context.Request.Headers.AcceptLanguage.FirstOrDefault();

        if (!string.IsNullOrWhiteSpace(requestCulture) && supportedLanguages.Any(c => c.Name == requestCulture))
            cultureInfo = new CultureInfo(requestCulture);

        CultureInfo.CurrentCulture = cultureInfo;
        CultureInfo.CurrentUICulture = cultureInfo;

        await _next(context);
    }
}
