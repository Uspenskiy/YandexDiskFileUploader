using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using YandexDiskFileUploader.Core;

namespace YandexDiskFileUploader.Interfaces
{
    public interface IRequest
    {
        Result Run(); 
    }
}
