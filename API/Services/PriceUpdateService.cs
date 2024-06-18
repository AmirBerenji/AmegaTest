using API.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace API.Services
{
    public class PriceUpdateService : BackgroundService
    {
        private readonly ILogger<PriceUpdateService> _logger;
        private readonly IDataFetcher _dataFetcher;
        private readonly IHubContext<PriceHub> _hubContext;
        private readonly List<string> _instruments = new List<string> { "EURUSD", "USDJPY", "BTCUSD" };

        public PriceUpdateService(ILogger<PriceUpdateService> logger, IDataFetcher dataFetcher, IHubContext<PriceHub> hubContext)
        {
            _logger = logger;
            _dataFetcher = dataFetcher;
            _hubContext = hubContext;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            
            while (!stoppingToken.IsCancellationRequested)
            {
                foreach (var instrument in _instruments)
                {
                    var result = await _dataFetcher.GetCurrentPriceAsync(instrument);
                    if (result != null)
                    {
                        await _hubContext.Clients.Group(instrument).SendAsync("ReceivePriceUpdate", instrument, result);
                    }
                }
                await Task.Delay(10000, stoppingToken);
            }
        }
    }
}
