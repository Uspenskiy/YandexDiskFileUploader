using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using YandexDiskFileUploader.Core;
using YandexDiskFileUploader.Interfaces;
using YandexDiskFileUploader.Logger;
using YandexDiskFileUploader.Yandex;

namespace YandexDiskFileUploader
{
    public class ConsoleHostedService : IHostedService
    {
        private readonly ILogger _logger;
        private readonly IHostApplicationLifetime _appLifetime;
        private readonly IConfiguration _configuration;

        public ConsoleHostedService(ILogger<ConsoleHostedService> logger, 
            IHostApplicationLifetime appLifetime, 
            IConfiguration configuration)
        {
            _logger = logger;
            _appLifetime = appLifetime;
            _configuration = configuration;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var source = _configuration["s"];
            var destination = _configuration["d"];
            if (source == null || destination == null)
            {
                ConsoleLogger.Error(0, $"Не заданы параметры конмандной строки");
                return;
            }
            if (!Directory.Exists(source))
            {
                ConsoleLogger.Error(0, $"Папка отсутствует");
                return;
            }

            var direcroryCreater = new YandexDirecroryCreate(new Uri(_configuration["BaseAddress"]), _configuration["Token"]);
            IRequest request = direcroryCreater.GetRequest(destination);
            var result = request.Run();
            if (!result.Status)
            {
                ConsoleLogger.Error(0, result.Message);
                return;
            }

            var options = new ParallelOptions {CancellationToken = cancellationToken};
            var fileUpload = new YandexFileUpload(new Uri(_configuration["BaseAddress"]), _configuration["Token"], destination);
            var files = Directory.GetFiles(source)
                .Select(f => new KeyValuePair<string,IRequest>(f, fileUpload.GetRequest(f)));
            Parallel.ForEach(files, options, file =>
            {
                int id = ConsoleLogger.GetId();
                string fileName = Path.GetFileName(file.Key);
                ConsoleLogger.Write(id, $"{fileName} загружаеться");
                var resultCopy = file.Value.Run();
                if (resultCopy.Status)
                    ConsoleLogger.Write(id, $"{fileName} загружен    ");
                else
                    ConsoleLogger.Error(id, $"{fileName} {resultCopy.Message}");
            });
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {   
            _appLifetime.StopApplication();
        }
    }
}
