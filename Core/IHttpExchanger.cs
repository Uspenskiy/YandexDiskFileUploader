using System;
using System.Collections.Generic;
using System.Text;
using YandexDiskFileUploader.Interfaces;

namespace YandexDiskFileUploader.Core
{
    public interface IHttpExchanger
    {
        public IRequest GetRequest(string name);
    }
}
