using System.Security.Cryptography;
using System.Text;
using Cower.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Cower.Service.Services.Implementation;

public class YoomoneyService : IYoomoneyService
{
    private readonly string RECEIVER;
    private readonly string PAYMENT_TYPE;
    private readonly string SECRET;
    private readonly bool IS_DEVELOPMENT;
    
    private readonly ILogger<YoomoneyService> _logger;
    private readonly IHttpContextAccessor _contextAccessor;

    public YoomoneyService(
        ILogger<YoomoneyService> logger,
        IHttpContextAccessor contextAccessor)
    {
        _logger = logger;
        _contextAccessor = contextAccessor;

        IS_DEVELOPMENT = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";
        RECEIVER = Environment.GetEnvironmentVariable("YOOMONEY_RECEIVER")!;
        SECRET = Environment.GetEnvironmentVariable("YOOMONEY_SECRET")!;
        PAYMENT_TYPE = "AC";
    }

    public async Task<string> GetPaymentUrl(string label, decimal amount)
    {
        var host = _contextAccessor.HttpContext.Request.Scheme + "://" + _contextAccessor.HttpContext.Request.Host;
        var successUrl = host.TrimEnd('/') + "/#/payment-result";
        
        string requestUrl = $"https://yoomoney.ru/quickpay/confirm?" +
                            $"receiver={RECEIVER}&" +
                            $"quickpay-form=button&" +
                            $"paymentType={PAYMENT_TYPE}&" +
                            $"sum={amount}&" +
                            $"label={label}&" +
                            $"successURL={successUrl}";

        HttpClientHandler clientHandler = new HttpClientHandler();
        clientHandler.AllowAutoRedirect = false;
        var client = new HttpClient(clientHandler);
        HttpResponseMessage response = await client.GetAsync(requestUrl);
        
        if (response.StatusCode != System.Net.HttpStatusCode.Redirect)
        {
            throw new Exception($"Unexpected response status code when awaiting redirect from YooMoney: {response.StatusCode}");
        }
        
        return response.Headers.Location!.ToString();
    }

    public decimal CalculateCommission(decimal amount)
    {
        decimal commissionCoefficient = 0.03m;
        
        decimal commission = amount - (amount * (commissionCoefficient / (1 + commissionCoefficient)));

        return commission;
    }
    
    public bool ValidateNotification(YoomoneyNotification notification)
    {
        if (IS_DEVELOPMENT)
        {
            return true;
        }
        
        string data = $"{notification.NotificationType}&" +
                      $"{notification.OperationId}&" +
                      $"{notification.Amount:G29}&" +
                      $"{notification.Currency}&" +
                      $"{notification.Datetime}&" +
                      $"{notification.Sender}&" +
                      $"{notification.Codepro.ToString().ToLower()}&" +
                      $"{SECRET}&" +
                      $"{notification.Label}";
        
        byte[] dataBytes = Encoding.UTF8.GetBytes(data);
        byte[] hashBytes;
        using (var sha1 = SHA1.Create())
        {
            hashBytes = sha1.ComputeHash(dataBytes);
        }
        
        string expectedHash = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        
        return expectedHash == notification.Sha1Hash;
    }

}