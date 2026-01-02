using GyroLedger.CodeInterface.LudCache;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace GyroLedger.Kernel.LudCache;

public class LudCacheFreshnessPoller(
    IServiceProvider services,
    ILogger<LudCacheFreshnessPoller> logger
    ) : BackgroundService
{
    private int IntervalDelay => 15;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                DoRefresh(stoppingToken);
                await Task.Delay(IntervalDelay * 1000, stoppingToken);
            }
        }
        catch (TaskCanceledException)
        {
            // Expected when stoppingToken is cancelled.
        }
    }

    private void DoRefresh(CancellationToken stoppingToken)
    {
        try
        {
            using var _scope = services.CreateScope();
            _scope.ServiceProvider.GetRequiredService<ILudCacheRefreshAll>()?.RefreshAll(stoppingToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, @"refreshing table cache error");
        }
    }
}
