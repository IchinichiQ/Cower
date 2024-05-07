using System.Net;
using System.Text.Json;
using Cower.Web.Models;
using Microsoft.AspNetCore.Diagnostics;

namespace Cower.Web.StatusCodeHandlers;

public static class ServerErrorHandler
{
    public static async Task Invoke(HttpContext context)
    {
        var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
        var exception = exceptionHandlerPathFeature?.Error;
        
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        
        string errorMessage = "Произошла ошибка на стороне сервера";
        
        var isDevelopment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";
        if (isDevelopment && exception != null)
        {
            errorMessage = exception.Message;
        }
        
        var dto = new ErrorDTO(
            ErrorCodes.INTERNAL_SERVER_ERROR,
            errorMessage);
        var json = JsonSerializer.Serialize(dto, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });
        
        await context.Response.WriteAsync(json);
    }
}