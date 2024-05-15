using System.Security.Cryptography;
using System.Text;
using Cower.Domain.Models;
using Microsoft.Extensions.Logging;

namespace Cower.Service.Services.Implementation;

public class YoomoneyService : IYoomoneyService
{
    private readonly string RECEIVER;
    private readonly string PAYMENT_TYPE;
    private readonly string SUCCESS_URL;
    private readonly string SECRET;
    private readonly bool IS_DEVELOPMENT;
    
    private readonly ILogger<YoomoneyService> _logger;

    public YoomoneyService(ILogger<YoomoneyService> logger)
    {
        _logger = logger;

        IS_DEVELOPMENT = false;
        // RECEIVER = Environment.GetEnvironmentVariable("YOOMONEY_RECEIVER")!;
        RECEIVER = "410012099835767";
        //SECRET = Environment.GetEnvironmentVariable("YOOMONEY_SECRET")!;
        SECRET = "ogrLBtT6Na3nDXh8WNjpJzBV";
        PAYMENT_TYPE = "AC";
        SUCCESS_URL = "https://185.233.187.57:8080/"; // TODO: Change
    }

    public async Task<string> GetPaymentUrl(string label, decimal amount)
    {
        string requestUrl = $"https://yoomoney.ru/quickpay/confirm?" +
                            $"receiver={RECEIVER}&" +
                            $"quickpay-form=button&" +
                            $"paymentType={PAYMENT_TYPE}&" +
                            $"sum={amount}&" +
                            $"label={label}&" +
                            $"successURL={SUCCESS_URL}";

        HttpClientHandler clientHandler = new HttpClientHandler();
        clientHandler.AllowAutoRedirect = false;
        var client = new HttpClient(clientHandler);
        HttpResponseMessage response = await client.GetAsync(requestUrl);

        // Проверяем статус ответа
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
                      $"{notification.Amount}&" +
                      $"{notification.Currency}&" +
                      $"{notification.Datetime}&" +
                      $"{notification.Sender}&" +
                      $"{notification.Codepro}&" +
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