using GameVault.BLL.ModelVM.Game;
using GameVault.BLL.Services.Abstraction;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace GameVault.PLL.Services
{
    public class SaleBackgroundService : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<SaleBackgroundService> _logger;

        private static bool _saleActive = false;
        private static DateTime _lastCheck = DateTime.MinValue;

        public static class SaleStatus
        {
            public static bool IsSaleActive { get; internal set; }
        }

        public SaleBackgroundService(IServiceScopeFactory serviceScopeFactory, ILogger<SaleBackgroundService> logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Sale Background Service started");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await UpdateSaleStatus();
                    await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error in Sale Background Service");
                    await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
                }
            }

            _logger.LogInformation("Sale Background Service stopped");
        }

        private async Task UpdateSaleStatus()
        {
            var now = DateTime.Now.TimeOfDay;
            var start = new TimeSpan(14, 0, 0);  
            var end = new TimeSpan(2, 0, 0);    

            bool saleShouldBeActive = (end < start)
                ? now >= start || now <= end
                : now >= start && now <= end;

            if (saleShouldBeActive != _saleActive || DateTime.UtcNow - _lastCheck > TimeSpan.FromHours(1))
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var gameServices = scope.ServiceProvider.GetRequiredService<IGameServices>();

                var (success, games) = await gameServices.GetAllGameDetailsAsync();
                if (success && games != null)
                {
                    foreach (var game in games)
                    {
                        if (saleShouldBeActive)
                            game.Price = game.Price * 0.8m;
                        else
                            game.Price = game.Price;
                    }

                    _logger.LogInformation($"Sale status updated: {(saleShouldBeActive ? "ACTIVE" : "INACTIVE")} for {games.Count()} games.");
                }
                else
                {
                    _logger.LogWarning("No games found or failed to fetch games for sale update.");
                }

                _saleActive = saleShouldBeActive;
                SaleStatus.IsSaleActive = saleShouldBeActive;
                _lastCheck = DateTime.UtcNow;
            }
        }
    }
}
