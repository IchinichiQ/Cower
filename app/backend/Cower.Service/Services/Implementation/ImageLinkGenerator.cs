using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;

namespace Cower.Service.Services.Implementation;

public class ImageLinkGenerator : IImageLinkGenerator
{
    private readonly ILogger<ImageLinkGenerator> _logger;
    private readonly LinkGenerator _linkGenerator;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ImageLinkGenerator(
        ILogger<ImageLinkGenerator> logger,
        LinkGenerator linkGenerator,
        IHttpContextAccessor httpContextAccessor)
    {
        _logger = logger;
        _linkGenerator = linkGenerator;
        _httpContextAccessor = httpContextAccessor;
    }

    public string GetImageLink(long id)
    {
        var context = _httpContextAccessor.HttpContext;
        if (context == null)
        {
            _logger.LogError("HttpContext is null");
            throw new InvalidOperationException("HttpContext is null");
        }

        var routeValues = new RouteValueDictionary { { "id", id } };
        var uri = _linkGenerator.GetUriByAddress(context, "GetImage", routeValues);

        return uri;
    }
}