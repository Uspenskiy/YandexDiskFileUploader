using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using YandexDiskFileUploader.Core;
using YandexDiskFileUploader.Interfaces;

namespace YandexDiskFileUploader.Yandex
{
    public class YandexFileUpload : IHttpExchanger
    {
        private readonly Uri _baseUri;
        private readonly string _token;
        private readonly string _destination;

        public YandexFileUpload(Uri baseUri, string token, string destination)
        {
            _baseUri = baseUri;
            _token = token;
            _destination = destination;
        }

        public IRequest GetRequest(string name)
        {
            HttpClient client = new HttpClient
            {
                BaseAddress = _baseUri,
                DefaultRequestHeaders =
                {
                    Authorization = new AuthenticationHeaderValue(_token),
                    Accept = { new MediaTypeWithQualityHeaderValue("application/json")}
                }
            };
            string path = $"{_destination}/{Path.GetFileName(name)}";
            return new FileUpload(client, name, path);
        }
    }
}
