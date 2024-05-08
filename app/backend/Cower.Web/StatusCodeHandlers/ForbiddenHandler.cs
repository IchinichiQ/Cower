using System.Net;
using System.Text.Json;
using Cower.Web.Models;

namespace Cower.Web.StatusCodeHandlers;

public static class FordbiddenHandler
{
    public static async Task Invoke(HttpContext context)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.Forbidden;

        var dto = new ErrorDTO(
            ErrorCodes.FORBIDDEN,
            "Доступ к ресурсу запрещен");
        var json = JsonSerializer.Serialize(dto, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        await context.Response.WriteAsync(json);
    }
}