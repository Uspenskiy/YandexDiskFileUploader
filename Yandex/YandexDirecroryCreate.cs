using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using YandexDiskFileUploader.Core;
using YandexDiskFileUploader.Interfaces;

namespace YandexDiskFileUploader.Yandex
{
    public class YandexDirecroryCreate : IHttpExchanger
    {
        private readonly Uri _baseUri;
        private readonly string _token;

        public YandexDirecroryCreate(Uri baseUri, string token)
        {
            _baseUri = baseUri;
            _token = token;
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
            return new DirectoryCreate(client, name);
        }
    }
}
