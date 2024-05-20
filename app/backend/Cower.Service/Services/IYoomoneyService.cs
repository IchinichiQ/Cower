using Cower.Domain.Models;

namespace Cower.Service.Services;

public interface IYoomoneyService
{
    public Task<string> GetPaymentUrl(string label, decimal amount);
    public decimal CalculateCommission(decimal amount);
    public bool ValidateNotification(YoomoneyNotification notification);
}