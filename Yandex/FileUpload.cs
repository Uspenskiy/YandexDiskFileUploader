using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using YandexDiskFileUploader.Core;
using YandexDiskFileUploader.Interfaces;

namespace YandexDiskFileUploader.Yandex
{
    public class FileUpload : IRequest
    {
        private readonly HttpClient _client;
        private readonly string _fileNameSource;
        private readonly string _fileNameDestination;

        public FileUpload(HttpClient client, string fileNameSource, string fileNameDestination)
        {
            _client = client;
            _fileNameSource = fileNameSource;
            _fileNameDestination = fileNameDestination;
        }

        public Result Run()
        {
            string path = $"resources/upload?path={_fileNameDestination}&overwrite=overwrite";
            try
            {
                var response = _client.GetAsync(path).Result;
                if (!response.IsSuccessStatusCode)
                    return new Result { Status = false, Message = "Не удалось получить ссылку для копирования" };
                var result = response.Content.ReadAsStringAsync().Result;
                var body = JsonSerializer.Deserialize<YandexResponse>(result);
                var httpContent = new StreamContent(File.OpenRead(_fileNameSource));
                response = _client.PutAsync(body.href, httpContent).Result;
                if (!response.IsSuccessStatusCode)
                    return new Result { Status = false, Message = "Не удалось загрузить файл" };
                return new Result { Status = true };
            }
            catch (Exception exp)
            {
                return new Result { Status = false, Message = exp.Message };
            }
        }
    }
}
