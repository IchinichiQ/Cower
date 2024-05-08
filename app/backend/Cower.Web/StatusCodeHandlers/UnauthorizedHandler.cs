using System.Net;
using System.Text.Json;
using Cower.Web.Models;

namespace Cower.Web.StatusCodeHandlers;

public static class UnauthorizedHandler
{
    public static async Task Invoke(HttpContext context)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;

        var dto = new ErrorDTO(
            ErrorCodes.UNAUTHORIZED,
            "Не авторизован");
        var json = JsonSerializer.Serialize(dto, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        await context.Response.WriteAsync(json);
    }
}