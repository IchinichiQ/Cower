using Cower.Domain.Models.Booking;
using Cower.Service.Services;

namespace Cower.Web.HostedServices;

public class UpdateSuccessStatusHostedService : IHostedService, IDisposable
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<UpdatePaymentTimeoutStatusHostedService> _logger;
    private Timer _timer;

    public UpdateSuccessStatusHostedService(
        IServiceProvider serviceProvider, 
        ILogger<UpdatePaymentTimeoutStatusHostedService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{nameof(UpdateSuccessStatusHostedService)} started.");
        _timer = new Timer(UpdateBookingsStatus!, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
        
        return Task.CompletedTask;
    }

    private async void UpdateBookingsStatus(object state)
    {
        using var scope = _serviceProvider.CreateScope();
        var bookingService = scope.ServiceProvider.GetRequiredService<IBookingService>();

        var updated = await bookingService.UpdateSuccessStatus();

        if (updated > 0)
        {
            _logger.LogInformation($"Moved {updated} bookings to status {nameof(BookingStatus.Success)}");
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{nameof(UpdateSuccessStatusHostedService)} stopped.");
        _timer?.Change(Timeout.Infinite, 0);
        
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}