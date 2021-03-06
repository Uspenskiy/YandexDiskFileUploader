using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using YandexDiskFileUploader.Extensions;

namespace YandexDiskFileUploader
{
    class Program
    {
        private static CancellationTokenSource _cancellationTokenSource;
        static async Task Main(string[] args)
        {
            _cancellationTokenSource = new CancellationTokenSource();
            Console.CancelKeyPress += (sender, e) => _cancellationTokenSource.Cancel();
            using IHost host = CreateHostBuilder(args).Build();
            await host.RunAsync(_cancellationTokenSource.Token);
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
            => Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((hostingContext, config) =>
            {
                config.Sources.Clear();
                config.AddJsonFile("appsettings.json", true, true)
                    .AddCommandLineParam(args);
            })
            .ConfigureLogging(logging =>
            {
                logging.AddFilter("Microsoft", LogLevel.Warning)
                    .AddFilter("System", LogLevel.Warning)
                    .AddConsole();
            })
            .ConfigureServices((hostContext, services) =>
            {
                services.AddHostedService<ConsoleHostedService>();
            });
    }
}
