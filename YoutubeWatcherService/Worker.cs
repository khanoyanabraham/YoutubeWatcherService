using Newtonsoft.Json.Linq;
using System.Collections.Concurrent;
using System.Net.Http.Json;
using YoutubeWatcherService.Watcher;

namespace YoutubeWatcherService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }
        public override Task StartAsync(CancellationToken cancellationToken)
        {
            return base.StartAsync(cancellationToken);
        }
        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            await base.StopAsync(cancellationToken);
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                List<Task> watchListTasks = new List<Task>();

                if (_logger.IsEnabled(LogLevel.Information))
                {
                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                }
                using HttpClient client = new();
                var model = await client.GetFromJsonAsync<ProxyModel>("https://proxy-seller.com/personal/api/v1/c2e9f31d2c04f9a501dce0b6c971a907/proxy/list/ipv6");
                var russia = model.data.items.Where(x => x.country == "Russia").ToList();
                int indexer = 0;
                for (int i = 0; i < 20; i++)
                {
                    Task t = Task.Run(async () =>
                    {
                        Interlocked.Increment(ref indexer);
                        using var watchUnit = new WatchUnit("https://www.youtube.com/watch?v=vfl-T_VYt7M", russia[indexer].ip, 120);
                        await watchUnit.WatchAsync(stoppingToken);
                    }, stoppingToken);
                    await Task.Delay(100);
                    watchListTasks.Add(t);
                }
                 Task.WaitAll(watchListTasks.ToArray());
                Console.WriteLine("Done All");
                await Task.Delay(10000);
                indexer = 0;
            }
        }
    }
}
