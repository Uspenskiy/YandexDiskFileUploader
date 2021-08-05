using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using YandexDiskFileUploader.Core;
using YandexDiskFileUploader.Interfaces;

namespace YandexDiskFileUploader.Yandex
{
    public class DirectoryCreate : IRequest
    {
        private readonly HttpClient _client;
        private readonly string _name;

        public DirectoryCreate(HttpClient client, string name)
        {
            _client = client;
            _name = $"resources?path={name}";
        }

        public Result Run()
        {
            try
            {
                var response = _client.GetAsync(_name).Result;
                if (response.IsSuccessStatusCode)
                    return new Result { Status = true };
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    response =  _client.PutAsync(_name, null).Result;
                    return new Result { Status = response.StatusCode == HttpStatusCode.Created };
                }
                return new Result { Status = false, Message = "Не удалось создать папку" };
            }
            catch (Exception exp)
            {
                return new Result { Status = false, Message = exp.Message };
            }
        }
    }
}
